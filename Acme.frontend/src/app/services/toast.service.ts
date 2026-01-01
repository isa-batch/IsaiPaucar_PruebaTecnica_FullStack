import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  constructor(private toastr: ToastrService) {}

  success(message: string, title: string = 'Éxito') {
    this.toastr.success(message, title, {
      timeOut: 3000,
      progressBar: true,
      closeButton: true
    });
  }

  error(message: string, title: string = 'Error') {
    this.toastr.error(message, title, {
      timeOut: 5000,
      progressBar: true,
      closeButton: true
    });
  }

  warning(message: string, title: string = 'Advertencia') {
    this.toastr.warning(message, title, {
      timeOut: 4000,
      progressBar: true,
      closeButton: true
    });
  }

  info(message: string, title: string = 'Información') {
    this.toastr.info(message, title, {
      timeOut: 3000,
      progressBar: true,
      closeButton: true
    });
  }

  showErrors(errors: string[]) {
    errors.forEach(error => this.error(error));
  }
}
