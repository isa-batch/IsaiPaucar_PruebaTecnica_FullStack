using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DataBase
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string password_hash { get; set; } = string.Empty;

        // Campos de auditoría
        public int estado { get; set; } = 1; // 0 = eliminado, 1 = activo

        public DateTime creado_en { get; set; } = DateTime.UtcNow;

        public int? creado_por { get; set; }

        public DateTime? modificado_en { get; set; }

        public int? modificado_por { get; set; }

        // Navegación
        [ForeignKey("creado_por")]
        public virtual Usuario? UsuarioCreador { get; set; }

        [ForeignKey("modificado_por")]
        public virtual Usuario? UsuarioModificador { get; set; }

        public virtual ICollection<Proyecto> ProyectosCreados { get; set; } = new List<Proyecto>();
        public virtual ICollection<ProyectoUsuario> ProyectosParticipando { get; set; } = new List<ProyectoUsuario>();
        public virtual ICollection<InvitacionProyecto> Invitaciones { get; set; } = new List<InvitacionProyecto>();
        public virtual ICollection<TareaUsuario> TareasAsignadas { get; set; } = new List<TareaUsuario>();
    }
}
