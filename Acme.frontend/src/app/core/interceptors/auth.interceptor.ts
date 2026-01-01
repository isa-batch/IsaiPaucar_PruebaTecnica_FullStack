import { HttpHandlerFn, HttpRequest } from '@angular/common/http';

export function authInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn) {
  // TODO: Implementar lógica de autenticación
  return next(req);
}
