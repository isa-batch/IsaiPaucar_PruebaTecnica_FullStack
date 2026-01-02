import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IResponse } from '../interfaces/response.interface';
import { Tarea, TareaRequest } from '../interfaces/tarea.interface';

const baseUrl = environment.apiUrl;

@Injectable({ providedIn: 'root' })
export class TareaService {
  private http = inject(HttpClient);

  getByProyecto(proyectoId: number): Observable<IResponse<Tarea[]>> {
    return this.http.get<IResponse<Tarea[]>>(`${baseUrl}/Tarea/proyecto/${proyectoId}`);
  }

  getById(id: number): Observable<IResponse<Tarea>> {
    return this.http.get<IResponse<Tarea>>(`${baseUrl}/Tarea/${id}`);
  }

  create(request: TareaRequest): Observable<IResponse<Tarea>> {
    return this.http.post<IResponse<Tarea>>(`${baseUrl}/Tarea`, request);
  }

  update(id: number, request: TareaRequest): Observable<IResponse<Tarea>> {
    return this.http.put<IResponse<Tarea>>(`${baseUrl}/Tarea/${id}`, request);
  }

  delete(id: number): Observable<IResponse<boolean>> {
    return this.http.delete<IResponse<boolean>>(`${baseUrl}/Tarea/${id}`);
  }
}
