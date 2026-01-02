import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IResponse } from '../interfaces/response.interface';
import { 
  Proyecto, 
  ProyectoRequest, 
  InvitacionDto, 
  InvitacionRequest, 
  RespuestaInvitacionRequest 
} from '../interfaces/proyecto.interface';

const baseUrl = environment.apiUrl;

@Injectable({ providedIn: 'root' })
export class ProyectoService {
  private http = inject(HttpClient);

  getAll(): Observable<IResponse<Proyecto[]>> {
    return this.http.get<IResponse<Proyecto[]>>(`${baseUrl}/Proyecto`);
  }

  getById(id: number): Observable<IResponse<Proyecto>> {
    return this.http.get<IResponse<Proyecto>>(`${baseUrl}/Proyecto/${id}`);
  }

  create(request: ProyectoRequest): Observable<IResponse<Proyecto>> {
    return this.http.post<IResponse<Proyecto>>(`${baseUrl}/Proyecto`, request);
  }

  update(id: number, request: ProyectoRequest): Observable<IResponse<Proyecto>> {
    return this.http.put<IResponse<Proyecto>>(`${baseUrl}/Proyecto/${id}`, request);
  }

  delete(id: number): Observable<IResponse<boolean>> {
    return this.http.delete<IResponse<boolean>>(`${baseUrl}/Proyecto/${id}`);
  }

  enviarInvitacion(request: InvitacionRequest): Observable<IResponse<InvitacionDto>> {
    return this.http.post<IResponse<InvitacionDto>>(`${baseUrl}/Proyecto/invitar`, request);
  }

  getMisInvitaciones(): Observable<IResponse<InvitacionDto[]>> {
    return this.http.get<IResponse<InvitacionDto[]>>(`${baseUrl}/Proyecto/mis-invitaciones`);
  }

  aceptarInvitacion(invitacionId: number): Observable<IResponse<boolean>> {
    return this.http.post<IResponse<boolean>>(`${baseUrl}/Proyecto/invitaciones/${invitacionId}/aceptar`, {});
  }

  rechazarInvitacion(invitacionId: number): Observable<IResponse<boolean>> {
    return this.http.post<IResponse<boolean>>(`${baseUrl}/Proyecto/invitaciones/${invitacionId}/rechazar`, {});
  }

  responderInvitacion(request: RespuestaInvitacionRequest): Observable<IResponse<boolean>> {
    const endpoint = request.aceptar ? 'aceptar' : 'rechazar';
    return this.http.post<IResponse<boolean>>(`${baseUrl}/Proyecto/invitaciones/${request.invitacionId}/${endpoint}`, {});
  }

  removerMiembro(proyectoId: number, usuarioId: number): Observable<IResponse<boolean>> {
    return this.http.delete<IResponse<boolean>>(`${baseUrl}/Proyecto/${proyectoId}/miembro/${usuarioId}`);
  }
}
