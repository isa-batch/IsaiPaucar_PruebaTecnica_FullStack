using Microsoft.EntityFrameworkCore;
using Model.DataBase;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.id == id);
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.email == email);
        }

        public async Task<List<Usuario>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<Usuario>();

            var lowerSearchTerm = searchTerm.ToLower();

            return await _context.Usuarios
                .Where(u => u.nombre.ToLower().Contains(lowerSearchTerm) ||
                           u.email.ToLower().Contains(lowerSearchTerm))
                .Take(10)
                .ToListAsync();
        }
    }
}
