import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IResponse } from '../interfaces/response.interface';
import { Usuario } from '../interfaces/usuario.interface';

const baseUrl = environment.apiUrl;

@Injectable({ providedIn: 'root' })
export class UsuarioService {
  private http = inject(HttpClient);

  buscar(termino: string): Observable<IResponse<Usuario[]>> {
    return this.http.get<IResponse<Usuario[]>>(`${baseUrl}/Usuario/buscar`, {
      params: { termino }
    });
  }
}
