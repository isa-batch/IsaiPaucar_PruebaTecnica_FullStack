using Model.DataBase;

namespace Repository.Interfaces
{
    public interface IProyectoRepository
    {
        Task<List<Proyecto>> GetAllByUsuarioIdAsync(int usuarioId);
        Task<Proyecto?> GetByIdAsync(int id);
        Task<Proyecto> CreateAsync(Proyecto proyecto);
        Task<Proyecto> UpdateAsync(Proyecto proyecto);
        Task DeleteAsync(int id);
        Task<bool> IsUsuarioMiembroAsync(int proyectoId, int usuarioId);
        Task<bool> IsUsuarioPropietarioAsync(int proyectoId, int usuarioId);
        Task AddMiembroAsync(ProyectoUsuario proyectoUsuario);
        Task<List<InvitacionProyecto>> GetInvitacionesPendientesByUsuarioAsync(int usuarioId);
        Task<InvitacionProyecto?> GetInvitacionByIdAsync(int invitacionId);
        Task<InvitacionProyecto> CreateInvitacionAsync(InvitacionProyecto invitacion);
        Task UpdateInvitacionAsync(InvitacionProyecto invitacion);
    }
}
