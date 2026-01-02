import { Usuario } from './usuario.interface';

export interface Proyecto {
  id: number;
  nombre: string;
  descripcion?: string;
  propietarioId: number;
  propietario: Usuario;
  miembros: ProyectoMiembro[];
  cantidadMiembros: number;
  cantidadTareas: number;
  creadoEn: Date;
}

export interface ProyectoMiembro {
  id: number;
  proyectoId: number;
  usuarioId: number;
  usuario: Usuario;
  rol: number;
  rolNombre: string;
}

export interface ProyectoRequest {
  nombre: string;
  descripcion?: string;
}

export interface InvitacionDto {
  id: number;
  proyectoId: number;
  proyecto: Proyecto;
  usuarioId: number;
  usuario?: Usuario;
  estadoInvitacion: string;
  creadoEn: Date;
  creador?: Usuario;
}

export interface InvitacionRequest {
  proyectoId: number;
  emailUsuario: string;
}

export interface RespuestaInvitacionRequest {
  invitacionId: number;
  aceptar: boolean;
}

export enum RolProyecto {
  OWNER = 1,
  MEMBER = 2
}

export enum EstadoInvitacion {
  PENDIENTE = 'PENDIENTE',
  ACEPTADA = 'ACEPTADA',
  RECHAZADA = 'RECHAZADA'
}
