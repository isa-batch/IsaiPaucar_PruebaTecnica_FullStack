using Model.DTO.Usuario;

namespace Model.DTO.Proyecto
{
    public class InvitacionDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public ProyectoDto? Proyecto { get; set; }
        public int UsuarioId { get; set; }
        public UsuarioDto? Usuario { get; set; }
        public string EstadoInvitacion { get; set; } = string.Empty;
        public DateTime CreadoEn { get; set; }
        public UsuarioDto? Creador { get; set; }
    }
}
