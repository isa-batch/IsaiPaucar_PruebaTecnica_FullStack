import { Routes } from '@angular/router';
import { authenticatedGuard } from './auth/guards/authenticated.guard';
import { notAuthenticatedGuard } from './auth/guards/not-authenticated.guard';

export const routes: Routes = [
  {
    path: 'auth',
    canActivate: [notAuthenticatedGuard],
    loadChildren: () => import('./auth/auth.routes').then(m => m.authRoutes)
  },
  {
    path: '',
    canActivate: [authenticatedGuard],
    loadChildren: () => import('./front/front.routes').then(m => m.frontRoutes)
  },
  {
    path: '**',
    redirectTo: 'auth/login'
  }
];
