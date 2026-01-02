import { Component, signal, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';
import { AvatarModule } from 'primeng/avatar';
import { MenuModule } from 'primeng/menu';
import { TooltipModule } from 'primeng/tooltip';
import { MenuItem } from 'primeng/api';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ButtonModule,
    AvatarModule,
    MenuModule,
    TooltipModule
  ],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent {
  sidebarOpen = true;
  isLargeScreen = false;
  openSubMenu: string | null = null;
  public Math = Math;

  currentUser: any;
  userMenuItems: MenuItem[] = [];
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
    private router: Router
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
