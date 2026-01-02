import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProyectoService } from '../../../../services/proyecto.service';
import { TareaService } from '../../../../services/tarea.service';
import { AuthService } from '../../../../services/auth.service';
import { ToastService } from '../../../../services/toast.service';
import { UsuarioService } from '../../../../services/usuario.service';
import { Proyecto, InvitacionRequest } from '../../../../interfaces/proyecto.interface';
import { Tarea, TareaRequest, EstadoTarea, PrioridadTarea } from '../../../../interfaces/tarea.interface';
import { Usuario } from '../../../../interfaces/usuario.interface';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextarea } from 'primeng/inputtextarea';
import { ReactiveFormsModule, FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Select } from 'primeng/select';
import { AutoComplete } from 'primeng/autocomplete';
import { DropdownModule } from 'primeng/dropdown';
import { Menu } from 'primeng/menu';
import { TooltipModule } from 'primeng/tooltip';
import { MenuItem, ConfirmationService } from 'primeng/api';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-proyecto-detail',
  standalone: true,
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    TagModule,
    DialogModule,
    InputTextModule,
    InputTextarea,
    ReactiveFormsModule,
    FormsModule,
    Select,
    AutoComplete,
    Menu,
    TooltipModule,
    DropdownModule,
    ConfirmDialogModule
  ],
  templateUrl: './proyecto-detail.component.html',
  styleUrl: './proyecto-detail.component.css'
})
export class ProyectoDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private proyectoService = inject(ProyectoService);
  private tareaService = inject(TareaService);
  private authService = inject(AuthService);
  private toastService = inject(ToastService);
  private usuarioService = inject(UsuarioService);
  private fb = inject(FormBuilder);
  private confirmationService = inject(ConfirmationService);

  @ViewChild('taskMenu') taskMenu!: Menu;

  proyecto = signal<Proyecto | null>(null);
  tareas = signal<Tarea[]>([]);
  isOwner = signal(false);

  // Modals visibility
  showInviteModal = signal(false);
  showTaskModal = signal(false);
  showAssignModal = signal(false);
  editingTarea = signal<Tarea | null>(null);
  selectedTaskToAssign = signal<Tarea | null>(null);
  selectedAssigneeId = signal<number | null>(null);

  // Autocomplete
  usuariosFiltrados = signal<Usuario[]>([]);
  usuarioSeleccionado = signal<Usuario | null>(null);

  // Forms
  inviteForm: FormGroup;
  taskForm: FormGroup;

  EstadoTarea = EstadoTarea;
  PrioridadTarea = PrioridadTarea;

  // Task menu items
  taskMenuItems: MenuItem[] = [];

  constructor() {
    this.inviteForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });

    this.taskForm = this.fb.group({
      titulo: ['', [Validators.required, Validators.maxLength(150)]],
      descripcion: ['', [Validators.maxLength(500)]],
      estadoProgreso: [EstadoTarea.PENDIENTE, Validators.required],
      prioridad: [PrioridadTarea.MEDIA, Validators.required],
      usuariosAsignadosIds: [[]]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.params['id'];
    if (id) {
      this.loadData(Number(id));
    } else {
      this.router.navigate(['/proyectos']);
    }
  }

  loadData(id: number): void {
    this.proyectoService.getById(id).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.proyecto.set(res.data);
          this.checkOwner();
          this.loadTareas(id);
        } else {
          this.toastService.error('No se pudo cargar el proyecto');
          this.router.navigate(['/proyectos']);
        }
      },
      error: () => {
        this.toastService.error('Error al cargar proyecto');
        this.router.navigate(['/proyectos']);
      }
    });
  }

  loadTareas(proyectoId: number): void {
    this.tareaService.getByProyecto(proyectoId).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.tareas.set(res.data);
        }
      },
      error: () => {
        this.toastService.error('Error al cargar tareas');
      }
    });
  }

  checkOwner(): void {
    const user = this.authService.getCurrentUser();
    const proj = this.proyecto();
    if (proj && user && proj.propietarioId === user.id) {
      this.isOwner.set(true);
    }
  }

  openInviteModal(): void {
    this.inviteForm.reset();
    this.usuarioSeleccionado.set(null);
    this.usuariosFiltrados.set([]);
    this.showInviteModal.set(true);
  }

  buscarUsuarios(event: any): void {
    const query = event.query;

    if (!query || query.length < 2) {
      this.usuariosFiltrados.set([]);
      return;
    }

    this.usuarioService.buscar(query).subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.usuariosFiltrados.set(res.data);
        } else {
          this.usuariosFiltrados.set([]);
        }
      },
      error: () => {
        this.usuariosFiltrados.set([]);
      }
    });
  }

  enviarInvitacion(): void {
    const usuario = this.usuarioSeleccionado();
    if (!usuario) {
      this.toastService.warning('Selecciona un usuario de la lista');
      return;
    }

    const proj = this.proyecto();
    if (!proj) return;

    const request: InvitacionRequest = {
      proyectoId: proj.id,
      emailUsuario: usuario.email
    };

    this.proyectoService.enviarInvitacion(request).subscribe({
      next: (res) => {
        if (res.success) {
          this.toastService.success('Invitación enviada');
          this.showInviteModal.set(false);
          this.usuarioSeleccionado.set(null);
        } else {
          this.toastService.error(res.message);
        }
      },
      error: () => this.toastService.error('Error al enviar invitación')
    });
  }

  openTaskModal(): void {
    this.editingTarea.set(null);
    this.taskForm.reset({
      estadoProgreso: EstadoTarea.PENDIENTE,
      prioridad: PrioridadTarea.MEDIA,
      usuariosAsignadosIds: []
    });
    this.showTaskModal.set(true);
  }

  openEditTaskModal(tarea: Tarea): void {
    this.editingTarea.set(tarea);
    this.taskForm.patchValue({
      titulo: tarea.titulo,
      descripcion: tarea.descripcion,
      estadoProgreso: tarea.estadoProgreso,
      prioridad: tarea.prioridad,
      usuariosAsignadosIds: tarea.usuariosAsignados?.map(u => u.id) || []
    });
    this.showTaskModal.set(true);
  }

  guardarTarea(): void {
    if (this.taskForm.invalid) return;

    const proj = this.proyecto();
    if (!proj) return;

    const tarea = this.editingTarea();
    const formValue = this.taskForm.value;
    const request: TareaRequest = {
      Id: tarea?.id,
      ProyectoId: proj.id,
      Titulo: formValue.titulo,
      Descripcion: formValue.descripcion || '',
      EstadoProgreso: formValue.estadoProgreso,
      Prioridad: formValue.prioridad,
      UsuariosAsignadosIds: formValue.usuariosAsignadosIds || []
    };

    const operation = tarea
      ? this.tareaService.update(tarea.id, request)
      : this.tareaService.create(request);

    operation.subscribe({
      next: (res) => {
        if (res.success) {
          this.toastService.success(tarea ? 'Tarea actualizada' : 'Tarea creada');
          this.showTaskModal.set(false);
          this.loadTareas(proj.id);
        } else {
          this.toastService.error(res.message);
        }
      },
      error: () => this.toastService.error('Error al guardar tarea')
    });
  }

  showStatusMenu(event: Event, tarea: Tarea): void {
    this.taskMenuItems = [
      {
        label: 'Pendiente',
        icon: 'pi pi-clock',
        iconClass: 'text-warning-menu',
        command: () => this.cambiarEstadoTarea(tarea, EstadoTarea.PENDIENTE)
      },
      {
        label: 'En Progreso',
        icon: 'pi pi-spinner',
        iconClass: 'text-info-menu',
        command: () => this.cambiarEstadoTarea(tarea, EstadoTarea.EN_PROGRESO)
      },
      {
        label: 'Completada',
        icon: 'pi pi-check-circle',
        iconClass: 'text-success-menu',
        command: () => this.cambiarEstadoTarea(tarea, EstadoTarea.COMPLETADA)
      }
    ];

    this.taskMenu.toggle(event);
  }

  cambiarEstadoTarea(tarea: Tarea, nuevoEstado: EstadoTarea): void {
    const proj = this.proyecto();
    if (!proj) return;

    const request: TareaRequest = {
      Id: tarea.id,
      Titulo: tarea.titulo || '',
      Descripcion: tarea.descripcion || '',
      EstadoProgreso: nuevoEstado,
      Prioridad: tarea.prioridad || 2,
      ProyectoId: proj.id,
      UsuariosAsignadosIds: tarea.usuariosAsignados?.map(u => u.id) || []
    };

    this.tareaService.update(tarea.id, request).subscribe({
      next: (res) => {
        if (res.success) {
          this.toastService.success('Estado actualizado');
          this.loadTareas(proj.id);
        } else {
          this.toastService.error(res.message);
        }
      },
      error: () => this.toastService.error('Error al actualizar estado')
    });
  }

  getFirstName(fullName: string): string {
    if (!fullName) return '?';
    return fullName.split(' ')[0];
  }

  openAssignModal(tarea: Tarea): void {
    this.selectedTaskToAssign.set(tarea);

    const firstAssigned = tarea.usuariosAsignados?.[0];
    this.selectedAssigneeId.set(firstAssigned ? firstAssigned.id : null);
    this.showAssignModal.set(true);
  }

  asignarMiembro(): void {
    const tarea = this.selectedTaskToAssign();
    const proj = this.proyecto();
    const assigneeId = this.selectedAssigneeId();
    
    if (!tarea || !proj) return;

    const request: TareaRequest = {
      Id: tarea.id,
      Titulo: tarea.titulo || '',
      Descripcion: tarea.descripcion || '',
      EstadoProgreso: tarea.estadoProgreso || 2,
      Prioridad: tarea.prioridad || 2,
      ProyectoId: proj.id,
      
      UsuariosAsignadosIds: assigneeId ? [assigneeId] : []
    };

    this.tareaService.update(tarea.id, request).subscribe({
      next: (res) => {
        if (res.success) {
          this.toastService.success('Asignación actualizada correctamente');
          this.showAssignModal.set(false);
          this.loadTareas(proj.id);
        } else {
          this.toastService.error(res.message);
        }
      },
      error: () => this.toastService.error('Error al actualizar la asignación')
    });
  }

  get projectMembers() {
    const proj = this.proyecto();
    if (!proj) return [];
    
    const members = proj.miembros.map(m => m.usuario);
    
    const isOwnerInMembers = members.some(u => u.id === proj.propietario.id);
    if (!isOwnerInMembers) {
      members.unshift(proj.propietario);
    }
    
    return members;
  }

  eliminarTarea(tarea: Tarea): void {
    const proj = this.proyecto();
    if (!proj) return;

    this.confirmationService.confirm({
      message: `¿Estás seguro de que deseas eliminar la tarea "${tarea.titulo}"? Esta acción no se puede deshacer.`,
      header: 'Confirmar Eliminación',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Sí, eliminar',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'btn-delete-confirm',
      rejectButtonStyleClass: 'btn-cancel-confirm',
      accept: () => {
        this.tareaService.delete(tarea.id).subscribe({
          next: (res) => {
            if (res.success) {
              console.log('Tarea eliminada '+ tarea.id);
              this.toastService.success('Tarea eliminada correctamente');
              this.loadTareas(proj.id);
            } else {
              this.toastService.error(res.message);
            }
          },
          error: () => this.toastService.error('Error al intentar eliminar la tarea')
        });
      }
    });
  }

  getStatusSeverity(status: number): "success" | "secondary" | "info" | "warning" | "danger" | "contrast" | undefined {
    switch (status) {
      case EstadoTarea.COMPLETADA: return 'success';
      case EstadoTarea.EN_PROGRESO: return 'info';
      case EstadoTarea.PENDIENTE: return 'warning';
      default: return 'secondary';
    }
  }

  getPrioritySeverity(priority: number): "success" | "secondary" | "info" | "warning" | "danger" | "contrast" | undefined {
    switch (priority) {
      case PrioridadTarea.ALTA: return 'danger';
      case PrioridadTarea.MEDIA: return 'warning';
      case PrioridadTarea.BAJA: return 'info';
      default: return 'secondary';
    }
  }

  back(): void {
    this.router.navigate(['/proyectos']);
  }
}
