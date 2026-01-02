using System.ComponentModel.DataAnnotations;

namespace Model.DTO.Proyecto
{
    public class InvitacionRequest
    {
        [Required(ErrorMessage = "El ID del proyecto es requerido")]
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "El email del usuario es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string EmailUsuario { get; set; } = string.Empty;
    }
}
