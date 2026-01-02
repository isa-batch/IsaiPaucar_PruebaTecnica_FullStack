import { Injectable, signal, computed } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private _activeRequests = signal(0);

  // Computed signal to determine loading state
  public loading = computed(() => this._activeRequests() > 0);

  show(): void {
    this._activeRequests.update(count => count + 1);
  }

  hide(): void {
    // Ensure count doesn't go negative
    this._activeRequests.update(count => {
      const newCount = Math.max(0, count - 1);
      return newCount;
    });
  }

  reset(): void {
    // Force reset to 0 in case of desynchronization
    this._activeRequests.set(0);
  }

  getActiveRequests(): number {
    return this._activeRequests();
  }
}
