namespace Abstracciones.Modelos
{
    public class HorasExtras
    {
        public Guid EmpleadoId{ get; set; }
        public DateTime FechaRealizacion { get; set; }
        public decimal CantidadHoras { get; set; }
        public decimal TarifaHora { get; set; }
        public string Descripcion { get; set; }
        public int EstadoId { get; set; }
        public Guid ProcesadoPagoId { get; set; }
       
    }
    public class HorasExtrasResponse : HorasExtras
    {
        public Guid HorasExtrasId { get; set; }

    }
}
