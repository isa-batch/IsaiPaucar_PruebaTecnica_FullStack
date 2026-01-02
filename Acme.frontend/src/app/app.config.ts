import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import Lara from '@primeng/themes/lara';
import { es } from 'primelocale/es.json';
import { definePreset } from '@primeng/themes';
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { DialogService } from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';

import { routes } from './app.routes';
import { authInterceptor } from './auth/auth.interceptor';
import { loadingInterceptor } from './core/interceptors/loading.interceptor';
import { provideToastr } from 'ngx-toastr';

const AcmePreset = definePreset(Lara, {
  semantic: {
    primary: {
      50: '{blue.50}',
      100: '{blue.100}',
      200: '{blue.200}',
      300: '{blue.300}',
      400: '{blue.400}',
      500: '{blue.500}',
      600: '{blue.600}',
      700: '{blue.700}',
      800: '{blue.800}',
      900: '{blue.900}',
      950: '{blue.950}'
    }
  }
});

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
      withFetch(),
      withInterceptors([authInterceptor, loadingInterceptor])
    ),
    provideAnimationsAsync(),
    providePrimeNG({
      translation: es,
      theme: {
        preset: AcmePreset,
        options: {
          darkModeSelector: '.acme-dark',
          cssLayer: {
            name: 'primeng',
            order: 'theme, base, primeng'
          }
        }
      }
    }),
    provideToastr({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
      progressBar: true,
      closeButton: true
    }),
    DialogService,
    MessageService,
    ConfirmationService
  ]
};
