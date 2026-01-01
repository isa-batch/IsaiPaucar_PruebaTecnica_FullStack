import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';

export const notAuthenticatedGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  // TODO: Implementar lógica de autenticación
  return true;
};
