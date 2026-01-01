using AutoMapper;
using Infraestructure.Helpers;
using Model.Common;
using Model.DataBase;
using Model.DTO.Tarea;
using Repository.Interfaces;

namespace Domain
{
    public class TareaDomain
    {
        private readonly ITareaRepository _tareaRepository;
        private readonly IProyectoRepository _proyectoRepository;
        private readonly IMapper _mapper;
        private readonly ClaimsHelper _claimsHelper;

        public TareaDomain(
            ITareaRepository tareaRepository,
            IProyectoRepository proyectoRepository,
            IMapper mapper,
            ClaimsHelper claimsHelper)
        {
            _tareaRepository = tareaRepository;
            _proyectoRepository = proyectoRepository;
            _mapper = mapper;
            _claimsHelper = claimsHelper;
        }

        public async Task<Response<List<TareaDto>>> GetAllByProyectoAsync(int proyectoId)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();

                // Verificar que el usuario tenga acceso al proyecto
                var tieneAcceso = await _proyectoRepository.IsUsuarioPropietarioAsync(proyectoId, usuarioId) ||
                                 await _proyectoRepository.IsUsuarioMiembroAsync(proyectoId, usuarioId);

                if (!tieneAcceso)
                {
                    return Response<List<TareaDto>>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "No tienes permiso para ver las tareas de este proyecto" }
                    );
                }

                var tareas = await _tareaRepository.GetAllByProyectoIdAsync(proyectoId);
                var tareasDto = _mapper.Map<List<TareaDto>>(tareas);

                return Response<List<TareaDto>>.SuccessResponse(
                    tareasDto,
                    "Tareas obtenidas exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<List<TareaDto>>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<List<TareaDto>>.ErrorResponse(
                    "Error al obtener tareas",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<TareaDto>> GetByIdAsync(int id)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var tarea = await _tareaRepository.GetByIdAsync(id);

                if (tarea == null)
                {
                    return Response<TareaDto>.ErrorResponse(
                        "Tarea no encontrada",
                        new List<string> { "La tarea no existe" }
                    );
                }

                // Verificar que el usuario tenga acceso al proyecto
                var tieneAcceso = await _proyectoRepository.IsUsuarioPropietarioAsync(tarea.proyecto_id, usuarioId) ||
                                 await _proyectoRepository.IsUsuarioMiembroAsync(tarea.proyecto_id, usuarioId);

                if (!tieneAcceso)
                {
                    return Response<TareaDto>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "No tienes permiso para ver esta tarea" }
                    );
                }

                var tareaDto = _mapper.Map<TareaDto>(tarea);

                return Response<TareaDto>.SuccessResponse(
                    tareaDto,
                    "Tarea obtenida exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<TareaDto>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<TareaDto>.ErrorResponse(
                    "Error al obtener tarea",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<TareaDto>> CreateAsync(TareaRequest request)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();

                // Verificar que el usuario tenga acceso al proyecto
                var tieneAcceso = await _proyectoRepository.IsUsuarioPropietarioAsync(request.ProyectoId, usuarioId) ||
                                 await _proyectoRepository.IsUsuarioMiembroAsync(request.ProyectoId, usuarioId);

                if (!tieneAcceso)
                {
                    return Response<TareaDto>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "No tienes permiso para crear tareas en este proyecto" }
                    );
                }

                var tarea = _mapper.Map<Tarea>(request);
                tarea.estado = 1;
                tarea.creado_en = DateTime.UtcNow;
                tarea.creado_por = usuarioId;

                await _tareaRepository.CreateAsync(tarea);

                // Asignar usuarios si se especificaron
                if (request.UsuariosAsignadosIds != null && request.UsuariosAsignadosIds.Any())
                {
                    foreach (var usuarioAsignadoId in request.UsuariosAsignadosIds)
                    {
                        // Verificar que el usuario sea miembro del proyecto
                        var esMiembro = await _proyectoRepository.IsUsuarioMiembroAsync(request.ProyectoId, usuarioAsignadoId) ||
                                       await _proyectoRepository.IsUsuarioPropietarioAsync(request.ProyectoId, usuarioAsignadoId);

                        if (esMiembro)
                        {
                            var tareaUsuario = new TareaUsuario
                            {
                                tarea_id = tarea.id,
                                usuario_id = usuarioAsignadoId,
                                creado_en = DateTime.UtcNow,
                                creado_por = usuarioId
                            };

                            await _tareaRepository.AsignarUsuarioAsync(tareaUsuario);
                        }
                    }
                }

                // Recargar tarea con todas las relaciones
                var tareaCreada = await _tareaRepository.GetByIdAsync(tarea.id);
                var tareaDto = _mapper.Map<TareaDto>(tareaCreada);

                return Response<TareaDto>.SuccessResponse(
                    tareaDto,
                    "Tarea creada exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<TareaDto>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<TareaDto>.ErrorResponse(
                    "Error al crear tarea",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<TareaDto>> UpdateAsync(int id, TareaRequest request)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var tarea = await _tareaRepository.GetByIdAsync(id);

                if (tarea == null)
                {
                    return Response<TareaDto>.ErrorResponse(
                        "Tarea no encontrada",
                        new List<string> { "La tarea no existe" }
                    );
                }

                // Verificar que el usuario tenga acceso al proyecto
                var tieneAcceso = await _proyectoRepository.IsUsuarioPropietarioAsync(tarea.proyecto_id, usuarioId) ||
                                 await _proyectoRepository.IsUsuarioMiembroAsync(tarea.proyecto_id, usuarioId);

                if (!tieneAcceso)
                {
                    return Response<TareaDto>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "No tienes permiso para actualizar esta tarea" }
                    );
                }

                tarea.titulo = request.Titulo;
                tarea.descripcion = request.Descripcion;
                tarea.estado_progreso = request.EstadoProgreso;
                tarea.prioridad = request.Prioridad;
                tarea.modificado_en = DateTime.UtcNow;
                tarea.modificado_por = usuarioId;

                await _tareaRepository.UpdateAsync(tarea);

                // Actualizar asignaciones si se especificaron
                if (request.UsuariosAsignadosIds != null)
                {
                    // Obtener asignaciones actuales
                    var asignacionesActuales = await _tareaRepository.GetAsignacionesByTareaIdAsync(id);
                    var usuariosActuales = asignacionesActuales.Select(a => a.usuario_id).ToList();

                    // Desasignar usuarios que ya no están en la lista
                    foreach (var asignacion in asignacionesActuales)
                    {
                        if (!request.UsuariosAsignadosIds.Contains(asignacion.usuario_id))
                        {
                            await _tareaRepository.DesasignarUsuarioAsync(id, asignacion.usuario_id);
                        }
                    }

                    // Asignar nuevos usuarios
                    foreach (var usuarioAsignadoId in request.UsuariosAsignadosIds)
                    {
                        if (!usuariosActuales.Contains(usuarioAsignadoId))
                        {
                            // Verificar que el usuario sea miembro del proyecto
                            var esMiembro = await _proyectoRepository.IsUsuarioMiembroAsync(tarea.proyecto_id, usuarioAsignadoId) ||
                                           await _proyectoRepository.IsUsuarioPropietarioAsync(tarea.proyecto_id, usuarioAsignadoId);

                            if (esMiembro)
                            {
                                var tareaUsuario = new TareaUsuario
                                {
                                    tarea_id = id,
                                    usuario_id = usuarioAsignadoId,
                                    creado_en = DateTime.UtcNow,
                                    creado_por = usuarioId
                                };

                                await _tareaRepository.AsignarUsuarioAsync(tareaUsuario);
                            }
                        }
                    }
                }

                var tareaActualizada = await _tareaRepository.GetByIdAsync(id);
                var tareaDto = _mapper.Map<TareaDto>(tareaActualizada);

                return Response<TareaDto>.SuccessResponse(
                    tareaDto,
                    "Tarea actualizada exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<TareaDto>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<TareaDto>.ErrorResponse(
                    "Error al actualizar tarea",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var tarea = await _tareaRepository.GetByIdAsync(id);

                if (tarea == null)
                {
                    return Response<bool>.ErrorResponse(
                        "Tarea no encontrada",
                        new List<string> { "La tarea no existe" }
                    );
                }

                // Verificar que el usuario tenga acceso al proyecto
                var tieneAcceso = await _proyectoRepository.IsUsuarioPropietarioAsync(tarea.proyecto_id, usuarioId) ||
                                 await _proyectoRepository.IsUsuarioMiembroAsync(tarea.proyecto_id, usuarioId);

                if (!tieneAcceso)
                {
                    return Response<bool>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "No tienes permiso para eliminar esta tarea" }
                    );
                }

                await _tareaRepository.DeleteAsync(id);

                return Response<bool>.SuccessResponse(
                    true,
                    "Tarea eliminada exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<bool>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<bool>.ErrorResponse(
                    "Error al eliminar tarea",
                    new List<string> { ex.Message }
                );
            }
        }
    }
}
