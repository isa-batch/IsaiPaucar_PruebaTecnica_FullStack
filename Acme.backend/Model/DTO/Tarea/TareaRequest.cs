using System.ComponentModel.DataAnnotations;

namespace Model.DTO.Tarea
{
    public class TareaRequest
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "El ID del proyecto es requerido")]
        public int ProyectoId { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        public string? Descripcion { get; set; }

        [Range(1, 3, ErrorMessage = "El estado debe ser 1 (COMPLETADA), 2 (PENDIENTE) o 3 (EN_PROGRESO)")]
        public int EstadoProgreso { get; set; } = 2;

        [Range(1, 3, ErrorMessage = "La prioridad debe ser 1 (BAJA), 2 (MEDIA) o 3 (ALTA)")]
        public int Prioridad { get; set; } = 2;

        public List<int> UsuariosAsignadosIds { get; set; } = new();
    }
}
