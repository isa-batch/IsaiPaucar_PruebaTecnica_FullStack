using Model.DataBase;

namespace Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(int id);
        Task<Usuario?> GetByEmailAsync(string email);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<bool> EmailExistsAsync(string email);
        Task<List<Usuario>> SearchAsync(string searchTerm);
    }
}
