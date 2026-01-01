import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  imports: [],
  template: `
    <div class="min-h-screen bg-gray-100 p-8">
      <div class="max-w-7xl mx-auto">
        <h1 class="text-3xl font-bold mb-4">Bienvenido a Acme</h1>
        <p class="text-gray-600">Sistema en desarrollo</p>
      </div>
    </div>
  `,
  styles: ``
})
export class HomeComponent {
}
