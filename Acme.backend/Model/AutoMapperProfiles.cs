using AutoMapper;
using Model.DataBase;
using Model.DTO.Auth;
using Model.DTO.Proyecto;
using Model.DTO.Tarea;
using Model.DTO.Usuario;
using Model.Enums;

namespace Model
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // ===== USUARIO =====
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForMember(dest => dest.CreadoEn, opt => opt.MapFrom(src => src.creado_en));

            CreateMap<RegisterRequest, Usuario>()
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.password_hash, opt => opt.Ignore()) // Se asigna manualmente con hash
                .ForMember(dest => dest.id, opt => opt.Ignore())
                .ForMember(dest => dest.estado, opt => opt.Ignore())
                .ForMember(dest => dest.creado_en, opt => opt.Ignore())
                .ForMember(dest => dest.creado_por, opt => opt.Ignore())
                .ForMember(dest => dest.modificado_en, opt => opt.Ignore())
                .ForMember(dest => dest.modificado_por, opt => opt.Ignore());

            // ===== PROYECTO =====
            CreateMap<Proyecto, ProyectoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.descripcion))
                .ForMember(dest => dest.PropietarioId, opt => opt.MapFrom(src => src.propietario_id))
                .ForMember(dest => dest.Propietario, opt => opt.MapFrom(src => src.Propietario))
                .ForMember(dest => dest.Miembros, opt => opt.MapFrom(src => src.Miembros.Where(m => m.estado == 1)))
                .ForMember(dest => dest.CantidadMiembros, opt => opt.MapFrom(src => src.Miembros.Count(m => m.estado == 1)))
                .ForMember(dest => dest.CantidadTareas, opt => opt.MapFrom(src => src.Tareas.Count(t => t.estado == 1)))
                .ForMember(dest => dest.CreadoEn, opt => opt.MapFrom(src => src.creado_en))
                .ForMember(dest => dest.ModificadoEn, opt => opt.MapFrom(src => src.modificado_en));

            CreateMap<ProyectoUsuario, ProyectoMiembroDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.ProyectoId, opt => opt.MapFrom(src => src.proyecto_id))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.usuario_id))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.rol))
                .ForMember(dest => dest.RolNombre, opt => opt.MapFrom(src =>
                    src.rol == 1 ? "OWNER" : "MEMBER"));

            CreateMap<ProyectoRequest, Proyecto>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id ?? 0))
                .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.propietario_id, opt => opt.Ignore()) // Se asigna manualmente
                .ForMember(dest => dest.estado, opt => opt.Ignore())
                .ForMember(dest => dest.creado_en, opt => opt.Ignore())
                .ForMember(dest => dest.creado_por, opt => opt.Ignore())
                .ForMember(dest => dest.modificado_en, opt => opt.Ignore())
                .ForMember(dest => dest.modificado_por, opt => opt.Ignore());

            // ===== INVITACIÓN =====
            CreateMap<InvitacionProyecto, InvitacionDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.ProyectoId, opt => opt.MapFrom(src => src.proyecto_id))
                .ForMember(dest => dest.Proyecto, opt => opt.MapFrom(src => src.Proyecto))
                .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.usuario_id))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.Usuario))
                .ForMember(dest => dest.EstadoInvitacion, opt => opt.MapFrom(src => src.estado_invitacion))
                .ForMember(dest => dest.CreadoEn, opt => opt.MapFrom(src => src.creado_en))
                .ForMember(dest => dest.Creador, opt => opt.MapFrom(src => src.UsuarioCreador));

            // ===== TAREA =====
            CreateMap<Tarea, TareaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.ProyectoId, opt => opt.MapFrom(src => src.proyecto_id))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.titulo))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.descripcion))
                .ForMember(dest => dest.EstadoProgreso, opt => opt.MapFrom(src => src.estado_progreso))
                .ForMember(dest => dest.EstadoProgresoNombre, opt => opt.MapFrom(src =>
                    src.estado_progreso == 1 ? "COMPLETADA" :
                    src.estado_progreso == 2 ? "PENDIENTE" : "EN_PROGRESO"))
                .ForMember(dest => dest.Prioridad, opt => opt.MapFrom(src => src.prioridad))
                .ForMember(dest => dest.PrioridadNombre, opt => opt.MapFrom(src =>
                    src.prioridad == 1 ? "BAJA" :
                    src.prioridad == 2 ? "MEDIA" : "ALTA"))
                .ForMember(dest => dest.UsuariosAsignados, opt => opt.MapFrom(src =>
                    src.UsuariosAsignados.Select(tu => tu.Usuario)))
                .ForMember(dest => dest.CreadoEn, opt => opt.MapFrom(src => src.creado_en))
                .ForMember(dest => dest.ModificadoEn, opt => opt.MapFrom(src => src.modificado_en))
                .ForMember(dest => dest.Creador, opt => opt.MapFrom(src => src.UsuarioCreador));

            CreateMap<TareaRequest, Tarea>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id ?? 0))
                .ForMember(dest => dest.proyecto_id, opt => opt.MapFrom(src => src.ProyectoId))
                .ForMember(dest => dest.titulo, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.estado_progreso, opt => opt.MapFrom(src => src.EstadoProgreso))
                .ForMember(dest => dest.prioridad, opt => opt.MapFrom(src => src.Prioridad))
                .ForMember(dest => dest.estado, opt => opt.Ignore())
                .ForMember(dest => dest.creado_en, opt => opt.Ignore())
                .ForMember(dest => dest.creado_por, opt => opt.Ignore())
                .ForMember(dest => dest.modificado_en, opt => opt.Ignore())
                .ForMember(dest => dest.modificado_por, opt => opt.Ignore());
        }
    }
}
