import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { LoadingService } from '../services/loading.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  
  // Show loader
  loadingService.show();

  return next(req).pipe(
    finalize(() => {
      // Hide loader on success or error
      loadingService.hide();
    })
  );
};
