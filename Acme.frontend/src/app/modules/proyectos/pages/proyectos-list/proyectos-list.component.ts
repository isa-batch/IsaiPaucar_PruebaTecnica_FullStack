import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { TooltipModule } from 'primeng/tooltip';
import { ConfirmationService } from 'primeng/api';
import { InputTextModule } from 'primeng/inputtext';
import { InputTextarea } from 'primeng/inputtextarea';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProyectoService } from '../../../../services/proyecto.service';
import { ToastService } from '../../../../services/toast.service';
import { AuthService } from '../../../../services/auth.service';
import { Proyecto, ProyectoRequest } from '../../../../interfaces/proyecto.interface';

@Component({
  selector: 'app-proyectos-list',
  standalone: true,
  imports: [
    CommonModule,
    CardModule,
    ButtonModule,
    DialogModule,
    ConfirmDialogModule,
    TooltipModule,
    InputTextModule,
    InputTextarea,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './proyectos-list.component.html',
  styleUrl: './proyectos-list.component.css'
})
export class ProyectosListComponent implements OnInit {
  private confirmationService = inject(ConfirmationService);
  proyectos = signal<Proyecto[]>([]);
  showDialog = signal(false);
  proyectoForm: FormGroup;
  editingProyecto: Proyecto | null = null;

  constructor(
    private proyectoService: ProyectoService,
    private toastService: ToastService,
    private authService: AuthService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.proyectoForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(150)]],
      descripcion: ['', [Validators.maxLength(500)]]
    });
  }

  ngOnInit(): void {
    this.loadProyectos();
  }

  loadProyectos(): void {
    this.proyectoService.getAll().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.proyectos.set(response.data);
        }
      },
      error: (error) => {
        this.toastService.error('Error al cargar proyectos');
      }
    });
  }

  openCreateDialog(): void {
    this.editingProyecto = null;
    this.proyectoForm.reset();
    this.showDialog.set(true);
  }

  openEditDialog(proyecto: Proyecto): void {
    this.editingProyecto = proyecto;
    this.proyectoForm.patchValue({
      nombre: proyecto.nombre,
      descripcion: proyecto.descripcion
    });
    this.showDialog.set(true);
  }

  saveProyecto(): void {
    if (this.proyectoForm.invalid) {
      this.toastService.warning('Por favor complete los campos requeridos');
      return;
    }

    const request: ProyectoRequest = this.proyectoForm.value;
    const operation = this.editingProyecto
      ? this.proyectoService.update(this.editingProyecto.id, request)
      : this.proyectoService.create(request);

    operation.subscribe({
      next: (response) => {
        if (response.success) {
          this.toastService.success(
            this.editingProyecto ? 'Proyecto actualizado' : 'Proyecto creado'
          );
          this.showDialog.set(false);
          this.loadProyectos();
        } else {
          this.toastService.error(response.message);
        }
      },
      error: () => {
        this.toastService.error('Error al guardar proyecto');
      }
    });
  }

  viewProyecto(proyecto: Proyecto): void {
    this.router.navigate(['/proyectos', proyecto.id]);
  }

  deleteProyecto(proyecto: Proyecto, event?: MouseEvent): void {
    if (event) {
      event.stopPropagation();
    }
    this.confirmationService.confirm({
      message: `¿Estás seguro de que deseas eliminar el proyecto "${proyecto.nombre}"? Esta acción no se puede deshacer.`,
      header: 'Confirmar Eliminación',
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Sí, eliminar',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'btn-delete-confirm',
      rejectButtonStyleClass: 'btn-cancel-confirm',
      accept: () => {
        this.proyectoService.delete(proyecto.id).subscribe({
          next: (response) => {
            if (response.success) {
              this.toastService.success('Proyecto eliminado correctamente');
              this.loadProyectos();
            } else {
              this.toastService.error(response.message);
            }
          },
          error: () => {
            this.toastService.error('Error al intentar eliminar el proyecto');
          }
        });
      }
    });
  }

  isOwner(proyecto: Proyecto): boolean {
    const currentUser = this.authService.getCurrentUser();
    return !!(currentUser && proyecto.propietarioId === currentUser.id);
  }
}
