using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DataBase
{
    [Table("proyecto_usuarios")]
    public class ProyectoUsuario
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int proyecto_id { get; set; }

        [Required]
        public int usuario_id { get; set; }

        [Required]
        public int rol { get; set; } // 1 = OWNER, 2 = MEMBER

        // Campos de auditoría
        public int estado { get; set; } = 1; // 0 = eliminado, 1 = activo

        public DateTime creado_en { get; set; } = DateTime.UtcNow;

        public int? creado_por { get; set; }

        // Navegación
        [ForeignKey("proyecto_id")]
        public virtual Proyecto Proyecto { get; set; } = null!;

        [ForeignKey("usuario_id")]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey("creado_por")]
        public virtual Usuario? UsuarioCreador { get; set; }
    }
}
