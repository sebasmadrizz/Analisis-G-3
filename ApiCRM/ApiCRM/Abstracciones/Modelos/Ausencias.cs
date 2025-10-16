namespace Abstracciones.Modelos
{
    public class Ausencias
    {
        
        public Guid IdEmpleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Motivo { get; set; }
        public string TipoAusencia { get; set; }
        public bool Aprobada { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
    public class AusenciasResponse: Ausencias
    {
        public Guid AusenciaId { get; set; }
    }
}
