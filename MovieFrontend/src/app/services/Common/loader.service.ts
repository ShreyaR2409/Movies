import { Injectable, Signal, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoaderService {
  private isLoading = signal(false);

  get loaderSignal(): Signal<boolean> {
    return this.isLoading.asReadonly();
  }

  showLoader(): void {
    this.isLoading.set(true);
  }

  hideLoader(): void {
    this.isLoading.set(false);
  }
}
