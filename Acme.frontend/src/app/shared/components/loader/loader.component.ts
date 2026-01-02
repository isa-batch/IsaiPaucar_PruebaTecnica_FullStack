import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { LoadingService } from '../../../core/services/loading.service';

@Component({
  selector: 'app-loader',
  standalone: true,
  imports: [CommonModule, ProgressSpinnerModule],
  template: `
    <div *ngIf="loadingService.loading$ | async" class="loader-overlay">
      <div class="loader-container">
        <p-progressSpinner 
          styleClass="w-16 h-16" 
          strokeWidth="4" 
          animationDuration=".5s">
        </p-progressSpinner>
        <p>Cargando...</p>
      </div>
    </div>
  `,
  styles: [`
    .loader-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(255, 255, 255, 0.8);
      backdrop-filter: blur(4px);
      z-index: 9999;
      display: flex;
      align-items: center;
      justify-content: center;
      animation: fadeIn 0.2s ease-out;
    }

    .loader-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 1rem;
    }

    .loader-container p {
      color: #3b82f6; /* Blue-500 */
      font-weight: 600;
      font-size: 1.1rem;
      letter-spacing: 0.5px;
      animation: pulse 1.5s infinite;
    }

    @keyframes fadeIn {
      from { opacity: 0; }
      to { opacity: 1; }
    }

    @keyframes pulse {
      0%, 100% { opacity: 1; }
      50% { opacity: 0.5; }
    }
  `]
})
export class LoaderComponent {
  loadingService = inject(LoadingService);
}
