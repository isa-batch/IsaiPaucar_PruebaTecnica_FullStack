import { Routes } from '@angular/router';
import { LayoutComponent } from './pages/layout/layout.component';

export const frontRoutes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'home',
        loadComponent: () => import('./pages/home/home.component').then(m => m.HomeComponent)
      },
      {
        path: 'proyectos',
        loadChildren: () => import('../modules/proyectos/proyectos.routes').then(m => m.proyectosRoutes)
      },
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
      }
    ]
  }
];
