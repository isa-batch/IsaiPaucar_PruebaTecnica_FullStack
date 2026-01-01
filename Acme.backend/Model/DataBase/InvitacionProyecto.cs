using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DataBase
{
    [Table("invitaciones_proyecto")]
    public class InvitacionProyecto
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int proyecto_id { get; set; }

        [Required]
        public int usuario_id { get; set; }

        [Required]
        [StringLength(50)]
        public string estado_invitacion { get; set; } = "PENDIENTE"; // PENDIENTE, ACEPTADA, RECHAZADA

        // Campos de auditoría
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
