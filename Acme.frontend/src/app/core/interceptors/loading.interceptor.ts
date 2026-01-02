import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize, catchError, throwError } from 'rxjs';
import { LoadingService } from '../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);

  // Show loader
  loadingService.show();

  return next(req).pipe(
    catchError((error) => {
      // Ensure hide is called on error
      loadingService.hide();
      return throwError(() => error);
    }),
    finalize(() => {
      // Hide loader on completion (success or error)
      loadingService.hide();
    })
  );
};
