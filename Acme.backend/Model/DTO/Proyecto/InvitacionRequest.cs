using System.ComponentModel.DataAnnotations;

namespace Model.DTO.Proyecto
{
    public class InvitacionRequest
    {
        [Required(ErrorMessage = "El ID del proyecto es requerido")]
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UsuarioId { get; set; }
    }
}
