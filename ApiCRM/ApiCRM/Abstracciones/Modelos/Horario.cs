namespace Abstracciones.Modelos
{
    public class Horario
    {
        public string Nombre { get; set; }
        public TimeOnly Entrada { get; set; }
        public TimeOnly Salida { get; set; }
        public int EstadoId { get; set; }
    }
    public class HorarioResponse : Horario
    {
        public Guid HorarioId { get; set; }
    }
}
