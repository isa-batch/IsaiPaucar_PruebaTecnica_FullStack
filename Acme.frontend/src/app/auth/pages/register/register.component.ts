import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastService } from '../../../services/toast.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  registerForm: FormGroup;
  loading = false;
  error: string | null = null;
  showPassword = false;

  constructor() {
    this.registerForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    if (this.registerForm.invalid) return;

    this.loading = true;
    this.error = null;

    this.authService.register(this.registerForm.value).subscribe({
      next: (response) => {
        this.loading = false;
        this.toastService.success('¡Registro exitoso!', 'Bienvenido');
        this.registerForm.reset();
        setTimeout(() => {
          this.router.navigate(['/']); // Or login, depending on flow. Auto-login implies /
        }, 1000);
      },
      error: (error) => {
        this.loading = false;
        this.error = error.error?.error || 'Error al registrarse';
        this.toastService.error(this.error || 'Error desconocido', 'Error de registro');
      }
    });
  }
}
