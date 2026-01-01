using AutoMapper;
using Infraestructure.Helpers;
using Model.Common;
using Model.DataBase;
using Model.DTO.Proyecto;
using Model.Enums;
using Repository.Interfaces;

namespace Domain
{
    public class ProyectoDomain
    {
        private readonly IProyectoRepository _proyectoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly ClaimsHelper _claimsHelper;

        public ProyectoDomain(
            IProyectoRepository proyectoRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            ClaimsHelper claimsHelper)
        {
            _proyectoRepository = proyectoRepository;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _claimsHelper = claimsHelper;
        }

        public async Task<Response<List<ProyectoDto>>> GetAllAsync()
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var proyectos = await _proyectoRepository.GetAllByUsuarioIdAsync(usuarioId);
                var proyectosDto = _mapper.Map<List<ProyectoDto>>(proyectos);

                return Response<List<ProyectoDto>>.SuccessResponse(
                    proyectosDto,
                    "Proyectos obtenidos exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<List<ProyectoDto>>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<List<ProyectoDto>>.ErrorResponse(
                    "Error al obtener proyectos",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<ProyectoDto>> GetByIdAsync(int id)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var proyecto = await _proyectoRepository.GetByIdAsync(id);

                if (proyecto == null)
                {
                    return Response<ProyectoDto>.ErrorResponse(
                        "Proyecto no encontrado",
                        new List<string> { "El proyecto no existe" }
                    );
                }

                // Verificar que el usuario tenga acceso al proyecto
                var tieneAcceso = proyecto.propietario_id == usuarioId ||
                                 await _proyectoRepository.IsUsuarioMiembroAsync(id, usuarioId);

                if (!tieneAcceso)
                {
                    return Response<ProyectoDto>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "No tienes permiso para acceder a este proyecto" }
                    );
                }

                var proyectoDto = _mapper.Map<ProyectoDto>(proyecto);

                return Response<ProyectoDto>.SuccessResponse(
                    proyectoDto,
                    "Proyecto obtenido exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<ProyectoDto>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<ProyectoDto>.ErrorResponse(
                    "Error al obtener proyecto",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<ProyectoDto>> CreateAsync(ProyectoRequest request)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();

                var proyecto = _mapper.Map<Proyecto>(request);
                proyecto.propietario_id = usuarioId;
                proyecto.estado = 1;
                proyecto.creado_en = DateTime.UtcNow;
                proyecto.creado_por = usuarioId;

                await _proyectoRepository.CreateAsync(proyecto);

                // Agregar al propietario como miembro con rol OWNER
                var proyectoUsuario = new ProyectoUsuario
                {
                    proyecto_id = proyecto.id,
                    usuario_id = usuarioId,
                    rol = (int)RolProyecto.OWNER,
                    estado = 1,
                    creado_en = DateTime.UtcNow,
                    creado_por = usuarioId
                };

                await _proyectoRepository.AddMiembroAsync(proyectoUsuario);

                // Recargar proyecto con todas las relaciones
                var proyectoCreado = await _proyectoRepository.GetByIdAsync(proyecto.id);
                var proyectoDto = _mapper.Map<ProyectoDto>(proyectoCreado);

                return Response<ProyectoDto>.SuccessResponse(
                    proyectoDto,
                    "Proyecto creado exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<ProyectoDto>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<ProyectoDto>.ErrorResponse(
                    "Error al crear proyecto",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<ProyectoDto>> UpdateAsync(int id, ProyectoRequest request)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var proyecto = await _proyectoRepository.GetByIdAsync(id);

                if (proyecto == null)
                {
                    return Response<ProyectoDto>.ErrorResponse(
                        "Proyecto no encontrado",
                        new List<string> { "El proyecto no existe" }
                    );
                }

                // Solo el propietario puede actualizar el proyecto
                if (proyecto.propietario_id != usuarioId)
                {
                    return Response<ProyectoDto>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "Solo el propietario puede actualizar el proyecto" }
                    );
                }

                proyecto.nombre = request.Nombre;
                proyecto.descripcion = request.Descripcion;
                proyecto.modificado_en = DateTime.UtcNow;
                proyecto.modificado_por = usuarioId;

                await _proyectoRepository.UpdateAsync(proyecto);

                var proyectoActualizado = await _proyectoRepository.GetByIdAsync(id);
                var proyectoDto = _mapper.Map<ProyectoDto>(proyectoActualizado);

                return Response<ProyectoDto>.SuccessResponse(
                    proyectoDto,
                    "Proyecto actualizado exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<ProyectoDto>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<ProyectoDto>.ErrorResponse(
                    "Error al actualizar proyecto",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<bool>> DeleteAsync(int id)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var proyecto = await _proyectoRepository.GetByIdAsync(id);

                if (proyecto == null)
                {
                    return Response<bool>.ErrorResponse(
                        "Proyecto no encontrado",
                        new List<string> { "El proyecto no existe" }
                    );
                }

                // Solo el propietario puede eliminar el proyecto
                if (proyecto.propietario_id != usuarioId)
                {
                    return Response<bool>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "Solo el propietario puede eliminar el proyecto" }
                    );
                }

                await _proyectoRepository.DeleteAsync(id);

                return Response<bool>.SuccessResponse(
                    true,
                    "Proyecto eliminado exitosamente"
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
                    "Error al eliminar proyecto",
                    new List<string> { ex.Message }
                );
            }
        }

        // ===== INVITACIONES =====

        public async Task<Response<InvitacionDto>> InvitarUsuarioAsync(InvitacionRequest request)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var proyecto = await _proyectoRepository.GetByIdAsync(request.ProyectoId);

                if (proyecto == null)
                {
                    return Response<InvitacionDto>.ErrorResponse(
                        "Proyecto no encontrado",
                        new List<string> { "El proyecto no existe" }
                    );
                }

                // Solo el propietario puede invitar usuarios
                if (proyecto.propietario_id != usuarioId)
                {
                    return Response<InvitacionDto>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "Solo el propietario puede invitar usuarios" }
                    );
                }

                // Verificar que el usuario a invitar existe
                var usuarioInvitado = await _usuarioRepository.GetByEmailAsync(request.EmailUsuario);
                if (usuarioInvitado == null)
                {
                    return Response<InvitacionDto>.ErrorResponse(
                        "Usuario no encontrado",
                        new List<string> { "El usuario no existe" }
                    );
                }

                // Verificar que no sea ya miembro
                var esMiembro = await _proyectoRepository.IsUsuarioMiembroAsync(request.ProyectoId, usuarioInvitado.id);
                if (esMiembro)
                {
                    return Response<InvitacionDto>.ErrorResponse(
                        "Usuario ya es miembro",
                        new List<string> { "El usuario ya pertenece al proyecto" }
                    );
                }

                var invitacion = new InvitacionProyecto
                {
                    proyecto_id = request.ProyectoId,
                    usuario_id = usuarioInvitado.id,
                    estado_invitacion = EstadoInvitacion.PENDIENTE.ToString(),
                    creado_en = DateTime.UtcNow,
                    creado_por = usuarioId
                };

                await _proyectoRepository.CreateInvitacionAsync(invitacion);

                var invitacionDto = _mapper.Map<InvitacionDto>(invitacion);

                return Response<InvitacionDto>.SuccessResponse(
                    invitacionDto,
                    "Invitación enviada exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<InvitacionDto>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<InvitacionDto>.ErrorResponse(
                    "Error al enviar invitación",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<List<InvitacionDto>>> GetMisInvitacionesAsync()
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var invitaciones = await _proyectoRepository.GetInvitacionesPendientesByUsuarioAsync(usuarioId);
                var invitacionesDto = _mapper.Map<List<InvitacionDto>>(invitaciones);

                return Response<List<InvitacionDto>>.SuccessResponse(
                    invitacionesDto,
                    "Invitaciones obtenidas exitosamente"
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<List<InvitacionDto>>.ErrorResponse(
                    "No autorizado",
                    new List<string> { ex.Message }
                );
            }
            catch (Exception ex)
            {
                return Response<List<InvitacionDto>>.ErrorResponse(
                    "Error al obtener invitaciones",
                    new List<string> { ex.Message }
                );
            }
        }

        public async Task<Response<bool>> ResponderInvitacionAsync(int invitacionId, bool aceptar)
        {
            try
            {
                var usuarioId = _claimsHelper.GetUsuarioId();
                var invitacion = await _proyectoRepository.GetInvitacionByIdAsync(invitacionId);

                if (invitacion == null)
                {
                    return Response<bool>.ErrorResponse(
                        "Invitación no encontrada",
                        new List<string> { "La invitación no existe" }
                    );
                }

                // Verificar que la invitación sea para el usuario autenticado
                if (invitacion.usuario_id != usuarioId)
                {
                    return Response<bool>.ErrorResponse(
                        "Acceso denegado",
                        new List<string> { "Esta invitación no es para ti" }
                    );
                }

                // Verificar que esté pendiente
                if (invitacion.estado_invitacion != EstadoInvitacion.PENDIENTE.ToString())
                {
                    return Response<bool>.ErrorResponse(
                        "Invitación ya procesada",
                        new List<string> { "Esta invitación ya fue respondida" }
                    );
                }

                if (aceptar)
                {
                    invitacion.estado_invitacion = EstadoInvitacion.ACEPTADA.ToString();

                    // Agregar como miembro del proyecto
                    var proyectoUsuario = new ProyectoUsuario
                    {
                        proyecto_id = invitacion.proyecto_id,
                        usuario_id = usuarioId,
                        rol = (int)RolProyecto.MEMBER,
                        estado = 1,
                        creado_en = DateTime.UtcNow,
                        creado_por = usuarioId
                    };

                    await _proyectoRepository.AddMiembroAsync(proyectoUsuario);
                }
                else
                {
                    invitacion.estado_invitacion = EstadoInvitacion.RECHAZADA.ToString();
                }

                await _proyectoRepository.UpdateInvitacionAsync(invitacion);

                return Response<bool>.SuccessResponse(
                    true,
                    aceptar ? "Invitación aceptada exitosamente" : "Invitación rechazada"
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
                    "Error al responder invitación",
                    new List<string> { ex.Message }
                );
            }
        }
    }
}
