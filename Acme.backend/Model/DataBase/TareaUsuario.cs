using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DataBase
{
    [Table("tareas_usuarios")]
    public class TareaUsuario
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int tarea_id { get; set; }

        [Required]
        public int usuario_id { get; set; }

        // Campos de auditoría
        public DateTime creado_en { get; set; } = DateTime.UtcNow;

        public int? creado_por { get; set; }

        // Navegación
        [ForeignKey("tarea_id")]
        public virtual Tarea Tarea { get; set; } = null!;

        [ForeignKey("usuario_id")]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey("creado_por")]
        public virtual Usuario? UsuarioCreador { get; set; }
    }
}
