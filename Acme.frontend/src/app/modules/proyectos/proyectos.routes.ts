import { Routes } from '@angular/router';

export const proyectosRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./pages/proyectos-list/proyectos-list.component').then(m => m.ProyectosListComponent)
  },
  {
    path: 'invitaciones',
    loadComponent: () => import('./pages/invitaciones/invitaciones.component').then(m => m.InvitacionesComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./pages/proyecto-detail/proyecto-detail.component').then(m => m.ProyectoDetailComponent)
  }
];
