using System.ComponentModel.DataAnnotations;

namespace Model.DTO.Proyecto
{
    public class ProyectoRequest
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "El nombre del proyecto es requerido")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        public string? Descripcion { get; set; }
    }
}
