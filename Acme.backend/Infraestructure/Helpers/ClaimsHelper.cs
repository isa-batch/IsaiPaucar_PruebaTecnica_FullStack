using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infraestructure.Helpers
{
    public class ClaimsHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUsuarioId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }

            return int.Parse(userId);
        }

        public string GetUsuarioEmail()
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Usuario no autenticado");
            }

            return email;
        }

        public string GetUsuarioNombre()
        {
            var nombre = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
            return nombre ?? string.Empty;
        }
    }
}
