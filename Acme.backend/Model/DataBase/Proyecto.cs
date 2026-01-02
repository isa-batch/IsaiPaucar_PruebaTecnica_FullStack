using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DataBase
{
    [Table("proyectos")]
    public class Proyecto
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(150)]
        public string nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? descripcion { get; set; }

        [Required]
        public int propietario_id { get; set; }

        // Campos de auditoría
        public int estado { get; set; } = 1; // 0 = eliminado, 1 = activo

        public DateTime creado_en { get; set; } = DateTime.UtcNow;

        public int? creado_por { get; set; }

        public DateTime? modificado_en { get; set; }

        public int? modificado_por { get; set; }

        // Navegación
        [ForeignKey("propietario_id")]
        public virtual Usuario Propietario { get; set; } = null!;

        [ForeignKey("creado_por")]
        public virtual Usuario? UsuarioCreador { get; set; }

        [ForeignKey("modificado_por")]
        public virtual Usuario? UsuarioModificador { get; set; }

        public virtual ICollection<ProyectoUsuario> Miembros { get; set; } = new List<ProyectoUsuario>();
        public virtual ICollection<InvitacionProyecto> Invitaciones { get; set; } = new List<InvitacionProyecto>();
        public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
    }
}
