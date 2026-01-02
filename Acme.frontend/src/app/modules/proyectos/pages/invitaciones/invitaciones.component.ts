import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProyectoService } from '../../../../services/proyecto.service';
import { ToastService } from '../../../../services/toast.service';
import { InvitacionDto } from '../../../../interfaces/proyecto.interface';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-invitaciones',
  standalone: true,
  imports: [
    CommonModule, 
    TableModule, 
    ButtonModule, 
    TagModule, 
    ConfirmDialogModule
  ],
  providers: [ConfirmationService],
  templateUrl: './invitaciones.component.html',
  styleUrl: './invitaciones.component.css'
})
export class InvitacionesComponent implements OnInit {
  private proyectoService = inject(ProyectoService);
  private toastService = inject(ToastService);
  private confirmationService = inject(ConfirmationService);
  private router = inject(Router);

  invitaciones = signal<InvitacionDto[]>([]);
  loading = signal(false);

  ngOnInit(): void {
    this.cargarInvitaciones();
  }

  cargarInvitaciones(): void {
    this.loading.set(true);
    this.proyectoService.getMisInvitaciones().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.invitaciones.set(res.data);
        }
        this.loading.set(false);
      },
      error: () => {
        this.toastService.error('Error al cargar invitaciones');
        this.loading.set(false);
      }
    });
  }

  aceptarInvitacion(inv: InvitacionDto): void {
    this.confirmationService.confirm({
      message: `¿Deseas unirte al proyecto "${inv.proyecto.nombre}"?`,
      header: 'Aceptar Invitación',
      icon: 'pi pi-check-circle',
      acceptLabel: 'Sí, unirme',
      rejectLabel: 'Cancelar',
      acceptButtonStyleClass: 'btn-acme-confirm',
      rejectButtonStyleClass: 'btn-acme-secondary',
      accept: () => {
        this.proyectoService.aceptarInvitacion(inv.id).subscribe({
          next: (res) => {
            if (res.success) {
              this.toastService.success('¡Te has unido al proyecto!');
              this.router.navigate(['/proyectos', inv.proyectoId]);
            } else {
              this.toastService.error(res.message);
            }
          },
          error: () => this.toastService.error('Error al aceptar invitación')
        });
      }
    });
  }

  rechazarInvitacion(inv: InvitacionDto): void {
    this.confirmationService.confirm({
      message: `¿Estás seguro de que deseas rechazar la invitación al proyecto "${inv.proyecto.nombre}"?`,
      header: 'Rechazar Invitación',
      icon: 'pi pi-times-circle',
      acceptLabel: 'Rechazar',
      rejectLabel: 'Volver',
      acceptButtonStyleClass: 'btn-acme-reject',
      rejectButtonStyleClass: 'btn-acme-secondary',
      accept: () => {
        this.proyectoService.rechazarInvitacion(inv.id).subscribe({
          next: (res) => {
            if (res.success) {
              this.toastService.info('Invitación rechazada');
              this.cargarInvitaciones();
            } else {
              this.toastService.error(res.message);
            }
          },
          error: () => this.toastService.error('Error al rechazar invitación')
        });
      }
    });
  }
}
