import { Component, signal, HostListener, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, NavigationEnd } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { MenuModule } from 'primeng/menu';
import { Menu } from 'primeng/menu';
import { TooltipModule } from 'primeng/tooltip';
import { LoaderComponent } from '../../../shared/components/loader/loader.component';
import { MenuItem } from 'primeng/api';
import { AuthService } from '../../../services/auth.service';
import { LoadingService } from '../../../core/services/loading.service';
import { ProyectoService } from '../../../services/proyecto.service';
import { ToastService } from '../../../services/toast.service';
import { InvitacionDto } from '../../../interfaces/proyecto.interface';
import { filter, interval } from 'rxjs';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ButtonModule,
    AvatarModule,
    MenuModule,
    TooltipModule,
    LoaderComponent
  ],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit {
  @ViewChild('userMenu') userMenu!: Menu;
  @ViewChild('notificationsMenu') notificationsMenu!: Menu;

  sidebarOpen = true;
  isLargeScreen = false;
  openSubMenu: string | null = null;
  public Math = Math;

  currentUser: any;
  userMenuItems: MenuItem[] = [];
  notificationMenuItems: MenuItem[] = [];
  invitacionesPendientes = signal<InvitacionDto[]>([]);

  menuItems = [
    {
      label: 'Inicio',
      icon: 'pi pi-home',
      route: '/home'
    },
    {
      label: 'Proyectos',
      icon: 'pi pi-folder',
      route: '/proyectos'
    }
  ];

  constructor(
    private authService: AuthService,
    private router: Router,
    private loadingService: LoadingService,
    private proyectoService: ProyectoService,
    private toastService: ToastService
  ) {
    this.currentUser = this.authService.getCurrentUser();
    this.userMenuItems = [
      {
        label: 'Cerrar Sesión',
        icon: 'pi pi-sign-out',
        command: () => this.logout()
      }
    ];
    this.checkScreenSize();
  }

  ngOnInit() {
    // Reset loading state on each navigation
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      // Force reset loading state after navigation completes
      setTimeout(() => {
        if (this.loadingService.getActiveRequests() > 0) {
          console.warn('Resetting stuck loading state');
          this.loadingService.reset();
        }
      }, 1000);
    });

    // Cargar invitaciones pendientes al iniciar
    this.cargarInvitaciones();

    // Polling cada 30 segundos para actualizar invitaciones
    interval(30000).subscribe(() => {
      this.cargarInvitaciones();
    });
  }

  cargarInvitaciones(): void {
    this.proyectoService.getMisInvitaciones().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.invitacionesPendientes.set(res.data);
          this.actualizarMenuNotificaciones();
        }
      },
      error: (err) => {
        console.error('Error al cargar invitaciones:', err);
      }
    });
  }

  actualizarMenuNotificaciones(): void {
    this.notificationMenuItems = this.invitacionesPendientes().map(inv => ({
      label: inv.proyecto?.nombre || 'Proyecto',
      subtitle: `Invitación de ${inv.proyecto?.propietario?.nombre || 'un usuario'}`,
      invitacionId: inv.id,
      icon: 'pi pi-folder'
    }));
  }

  /* HostListener needed for resize, add import if missing */
  // NOTE: In a real scenario I should add the HostListener decorator to imports
  
  @HostListener('window:resize')
  onResize() {
    this.checkScreenSize();
  }
  
  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
    
    // Close submenu when collapsing sidebar on desktop
    if (!this.sidebarOpen && this.isLargeScreen) {
      this.openSubMenu = null;
    }
  }

  checkScreenSize() {
    const wasLargeScreen = this.isLargeScreen;
    this.isLargeScreen = window.innerWidth >= 1024;
    
    // Auto-open sidebar on desktop, auto-close on mobile
    if (!wasLargeScreen && this.isLargeScreen) {
      this.sidebarOpen = true;
    } else if (wasLargeScreen && !this.isLargeScreen) {
      this.sidebarOpen = false;
    }
  }

  toggleSubMenu(label: string) {
    if (this.openSubMenu === label) {
      this.openSubMenu = null;
    } else {
      this.openSubMenu = label;
    }
  }

  toggleUserMenu(event: Event): void {
    if (this.userMenu) {
      this.userMenu.toggle(event);
    }
  }

  toggleNotifications(event: Event): void {
    if (this.notificationsMenu) {
      this.notificationsMenu.toggle(event);
    }
  }

  aceptarInvitacion(invitacionId: number, event: Event): void {
    event.stopPropagation();

    this.proyectoService.responderInvitacion({ invitacionId, aceptar: true }).subscribe({
      next: (res) => {
        if (res.success) {
          this.toastService.success('Invitación aceptada');
          this.cargarInvitaciones();
          if (this.notificationsMenu) {
            this.notificationsMenu.hide();
          }
        } else {
          this.toastService.error(res.message);
        }
      },
      error: () => this.toastService.error('Error al aceptar invitación')
    });
  }

  rechazarInvitacion(invitacionId: number, event: Event): void {
    event.stopPropagation();

    this.proyectoService.responderInvitacion({ invitacionId, aceptar: false }).subscribe({
      next: (res) => {
        if (res.success) {
          this.toastService.info('Invitación rechazada');
          this.cargarInvitaciones();
          if (this.notificationsMenu) {
            this.notificationsMenu.hide();
          }
        } else {
          this.toastService.error(res.message);
        }
      },
      error: () => this.toastService.error('Error al rechazar invitación')
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }

  isActiveRoute(route: string): boolean {
    return this.router.url === route;
  }

  getInitials(user: any): string {
    if (!user) return '';
    const first = user.nombre ? user.nombre.charAt(0).toUpperCase() : '';
    const second = user.apellidos ? user.apellidos.charAt(0).toUpperCase() : '';
    return `${first}${second}`;
  }
}
