using Model.DataBase;

namespace Repository.Interfaces
{
    public interface ITareaRepository
    {
        Task<List<Tarea>> GetAllByProyectoIdAsync(int proyectoId);
        Task<Tarea?> GetByIdAsync(int id);
        Task<Tarea> CreateAsync(Tarea tarea);
        Task<Tarea> UpdateAsync(Tarea tarea);
        Task DeleteAsync(int id, int usuarioId);
        Task AsignarUsuarioAsync(TareaUsuario tareaUsuario);
        Task DesasignarUsuarioAsync(int tareaId, int usuarioId);
        Task<List<TareaUsuario>> GetAsignacionesByTareaIdAsync(int tareaId);
    }
}
