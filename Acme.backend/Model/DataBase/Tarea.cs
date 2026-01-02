using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DataBase
{
    [Table("tareas")]
    public class Tarea
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int proyecto_id { get; set; }

        [Required]
        [StringLength(200)]
        public string titulo { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? descripcion { get; set; }

        [Required]
        public int estado_progreso { get; set; } = 2; // 1 = COMPLETADA, 2 = PENDIENTE, 3 = EN_PROGRESO

        [Required]
        public int prioridad { get; set; } = 2; // 1 = BAJA, 2 = MEDIA, 3 = ALTA

        // Campos de auditoría
        public int estado { get; set; } = 1; // 0 = eliminado, 1 = activo

        public DateTime creado_en { get; set; } = DateTime.UtcNow;

        public int? creado_por { get; set; }

        public DateTime? modificado_en { get; set; }

        public int? modificado_por { get; set; }

        // Navegación
        [ForeignKey("proyecto_id")]
        public virtual Proyecto Proyecto { get; set; } = null!;

        [ForeignKey("creado_por")]
        public virtual Usuario? UsuarioCreador { get; set; }

        [ForeignKey("modificado_por")]
        public virtual Usuario? UsuarioModificador { get; set; }

        public virtual ICollection<TareaUsuario> UsuariosAsignados { get; set; } = new List<TareaUsuario>();
    }
}
