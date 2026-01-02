using Model.DTO.Usuario;

namespace Model.DTO.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public UsuarioDto Usuario { get; set; } = null!;
    }
}
