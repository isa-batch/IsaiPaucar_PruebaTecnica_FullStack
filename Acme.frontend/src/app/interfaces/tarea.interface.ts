import { Usuario } from './usuario.interface';

export interface Tarea {
  id: number;
  proyectoId: number;
  titulo: string;
  descripcion?: string;
  estadoProgreso: number;
  estadoProgresoNombre: string;
  prioridad: number;
  prioridadNombre: string;
  usuariosAsignados: Usuario[];
  creadoPor: number;
  usuarioCreador: Usuario;
  creadoEn: Date;
}

export interface TareaRequest {
  Id?: number;
  ProyectoId: number;
  Titulo: string;
  Descripcion?: string;
  EstadoProgreso: number;
  Prioridad: number;
  UsuariosAsignadosIds: number[];
}

export enum EstadoTarea {
  COMPLETADA = 1,
  PENDIENTE = 2,
  EN_PROGRESO = 3
}

export enum PrioridadTarea {
  BAJA = 1,
  MEDIA = 2,
  ALTA = 3
}
