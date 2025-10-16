namespace Abstracciones.Modelos
{
    public class Incapacidades
    {
        public Guid EmpleadoId { get; set; }
        public string TipoIncapacidad{ get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string InstitucionMedica { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
    public class IncapacidadesResponse : Incapacidades
    {
        public Guid IncapacidadId { get; set; }
    }
}
