using Model.DTO.Usuario;

namespace Model.DTO.Proyecto
{
    public class ProyectoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int PropietarioId { get; set; }
        public UsuarioDto? Propietario { get; set; }
        public int CantidadMiembros { get; set; }
        public int CantidadTareas { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime? ModificadoEn { get; set; }
    }
}
