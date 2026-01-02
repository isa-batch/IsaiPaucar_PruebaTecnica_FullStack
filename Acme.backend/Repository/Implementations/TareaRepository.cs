using Microsoft.EntityFrameworkCore;
using Model.DataBase;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class TareaRepository : ITareaRepository
    {
        private readonly AppDbContext _context;

        public TareaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Tarea>> GetAllByProyectoIdAsync(int proyectoId)
        {
            return await _context.Tareas
                .Include(t => t.UsuariosAsignados)
                    .ThenInclude(tu => tu.Usuario)
                .Include(t => t.UsuarioCreador)
                .Where(t => t.proyecto_id == proyectoId)
                .OrderByDescending(t => t.creado_en)
                .ToListAsync();
        }

        public async Task<Tarea?> GetByIdAsync(int id)
        {
            return await _context.Tareas
                .Include(t => t.Proyecto)
                .Include(t => t.UsuariosAsignados)
                    .ThenInclude(tu => tu.Usuario)
                .Include(t => t.UsuarioCreador)
                .FirstOrDefaultAsync(t => t.id == id);
        }

        public async Task<Tarea> CreateAsync(Tarea tarea)
        {
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();
            return tarea;
        }

        public async Task<Tarea> UpdateAsync(Tarea tarea)
        {
            _context.Entry(tarea).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return tarea;
        }

        public async Task DeleteAsync(int id, int usuarioId)
        {
            await _context.Tareas
                .Where(t => t.id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.estado, 0)
                    .SetProperty(t => t.modificado_por, usuarioId)
                    .SetProperty(t => t.modificado_en, DateTime.UtcNow));
        }

        public async Task AsignarUsuarioAsync(TareaUsuario tareaUsuario)
        {
            _context.TareaUsuarios.Add(tareaUsuario);
            await _context.SaveChangesAsync();
        }

        public async Task DesasignarUsuarioAsync(int tareaId, int usuarioId)
        {
            var asignacion = await _context.TareaUsuarios
                .FirstOrDefaultAsync(tu => tu.tarea_id == tareaId && tu.usuario_id == usuarioId);

            if (asignacion != null)
            {
                _context.TareaUsuarios.Remove(asignacion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TareaUsuario>> GetAsignacionesByTareaIdAsync(int tareaId)
        {
            return await _context.TareaUsuarios
                .Include(tu => tu.Usuario)
                .Where(tu => tu.tarea_id == tareaId)
                .ToListAsync();
        }
    }
}
