import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { ProyectoService } from '../../../services/proyecto.service';
import { Proyecto, EstadoInvitacion } from '../../../interfaces/proyecto.interface';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  private authService = inject(AuthService);
  private proyectoService = inject(ProyectoService);
  private router = inject(Router);

  userName: string = 'Usuario';
  totalProyectos: number = 0;
  pendingInvitations: number = 0;
  recentProyectos: Proyecto[] = [];
  loading: boolean = true;

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    // Get User Name
    const user = this.authService.getCurrentUser();
    if (user) {
      this.userName = user.nombre;
    }

    this.loading = true;

    // Fetch Projects
    this.proyectoService.getAll().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.totalProyectos = response.data.length;
          // Sort by ID desc (approximation of recent) or just take first 3 if recently modified logic isn't available
          this.recentProyectos = response.data.slice(0, 3); 
        }
        this.checkLoadingComplete();
      },
      error: () => this.checkLoadingComplete()
    });

    // Fetch Invitations
    this.proyectoService.getMisInvitaciones().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.pendingInvitations = response.data.filter(inv => inv.estadoInvitacion === EstadoInvitacion.PENDIENTE).length;
        }
        this.checkLoadingComplete();
      },
      error: () => this.checkLoadingComplete()
    });
  }
  
  // Simple check to turn off loading when requests "settle" - simplified for parallel calls without forkJoin for now
  private requestsCompleted = 0;
  private checkLoadingComplete() {
    this.requestsCompleted++;
    if (this.requestsCompleted >= 2) {
        this.loading = false;
    }
  }

  crearProyecto() {
    // Navigate to create project. Assuming route exists or modal. 
    // Plan said "Button to Nuevo Proyecto". 
    // If no dedicated create page, maybe redirect to projects list with a query param?
    // Looking at routes, only 'proyectos' and 'proyectos/:id' exist. 
    // Usually 'proyectos' list has the create button.
    this.router.navigate(['/proyectos']); 
  }
}
