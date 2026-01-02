import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastService } from '../../../services/toast.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private toastService = inject(ToastService);

  loginForm: FormGroup;
  loading = false;
  error: string | null = null;
  success: string | null = null;
  showPassword = false;

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      remember: [true]
    });
  }

  ngOnInit(): void {
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.loading = true;
    this.error = null;
    this.success = null;

    this.authService.login(this.loginForm.value).subscribe({
      next: (response) => {
        this.loading = false;
        this.toastService.success('¡Inicio de sesión exitoso!', 'Bienvenido');
        this.loginForm.reset();
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 1000);
      },
      error: (error) => {
        this.loading = false;
        this.toastService.error(error.error?.error || 'Error al iniciar sesión', 'Error de autenticación');
      }
    });
  }
}
