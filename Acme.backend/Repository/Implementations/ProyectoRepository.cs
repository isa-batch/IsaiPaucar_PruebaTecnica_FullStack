using Microsoft.EntityFrameworkCore;
using Model.DataBase;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly AppDbContext _context;

        public ProyectoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Proyecto>> GetAllByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Proyectos
                .Include(p => p.Propietario)
                .Include(p => p.Miembros.Where(m => m.estado == 1))
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Tareas.Where(t => t.estado == 1))
                .Where(p => p.propietario_id == usuarioId ||
                           p.Miembros.Any(m => m.usuario_id == usuarioId && m.estado == 1))
                .ToListAsync();
        }

        public async Task<Proyecto?> GetByIdAsync(int id)
        {
            return await _context.Proyectos
                .Include(p => p.Propietario)
                .Include(p => p.Miembros.Where(m => m.estado == 1))
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Tareas.Where(t => t.estado == 1))
                .FirstOrDefaultAsync(p => p.id == id);
        }

        public async Task<Proyecto> CreateAsync(Proyecto proyecto)
        {
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();
            return proyecto;
        }

        public async Task<Proyecto> UpdateAsync(Proyecto proyecto)
        {
            _context.Proyectos.Update(proyecto);
            await _context.SaveChangesAsync();
            return proyecto;
        }

        public async Task DeleteAsync(int id)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto != null)
            {
                proyecto.estado = 0; // Eliminación lógica
                proyecto.modificado_en = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUsuarioMiembroAsync(int proyectoId, int usuarioId)
        {
            return await _context.ProyectoUsuarios
                .AnyAsync(pu => pu.proyecto_id == proyectoId &&
                               pu.usuario_id == usuarioId &&
                               pu.estado == 1);
        }

        public async Task<bool> IsUsuarioPropietarioAsync(int proyectoId, int usuarioId)
        {
            return await _context.Proyectos
                .AnyAsync(p => p.id == proyectoId && p.propietario_id == usuarioId);
        }

        public async Task AddMiembroAsync(ProyectoUsuario proyectoUsuario)
        {
            _context.ProyectoUsuarios.Add(proyectoUsuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<InvitacionProyecto>> GetInvitacionesPendientesByUsuarioAsync(int usuarioId)
        {
            return await _context.InvitacionesProyecto
                .Include(i => i.Proyecto)
                    .ThenInclude(p => p.Propietario)
                .Include(i => i.UsuarioCreador)
                .Where(i => i.usuario_id == usuarioId && i.estado_invitacion == "PENDIENTE")
                .ToListAsync();
        }

        public async Task<InvitacionProyecto?> GetInvitacionByIdAsync(int invitacionId)
        {
            return await _context.InvitacionesProyecto
                .Include(i => i.Proyecto)
                .FirstOrDefaultAsync(i => i.id == invitacionId);
        }

        public async Task<InvitacionProyecto> CreateInvitacionAsync(InvitacionProyecto invitacion)
        {
            _context.InvitacionesProyecto.Add(invitacion);
            await _context.SaveChangesAsync();
            return invitacion;
        }

        public async Task UpdateInvitacionAsync(InvitacionProyecto invitacion)
        {
            _context.InvitacionesProyecto.Update(invitacion);
            await _context.SaveChangesAsync();
        }
    }
}
