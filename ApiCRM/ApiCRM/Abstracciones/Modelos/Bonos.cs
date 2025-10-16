namespace Abstracciones.Modelos
{
    public class Bonos
    {
        public Guid EmpleadoId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string Descripcion { get; set; }
        public int EstadoId{ get; set; }
        public Guid ProcesadoPagoId { get; set; }
    }
    public class BonosResponse : Bonos
    {
        public Guid BonosId { get; set; }
    }
}
