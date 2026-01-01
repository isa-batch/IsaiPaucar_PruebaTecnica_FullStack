using Model.DTO.Usuario;

namespace Model.DTO.Tarea
{
    public class TareaDto
    {
        public int Id { get; set; }
        public int ProyectoId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int EstadoProgreso { get; set; }
        public string EstadoProgresoNombre { get; set; } = string.Empty;
        public int Prioridad { get; set; }
        public string PrioridadNombre { get; set; } = string.Empty;
        public List<UsuarioDto> UsuariosAsignados { get; set; } = new();
        public DateTime CreadoEn { get; set; }
        public DateTime? ModificadoEn { get; set; }
        public UsuarioDto? Creador { get; set; }
    }
}
