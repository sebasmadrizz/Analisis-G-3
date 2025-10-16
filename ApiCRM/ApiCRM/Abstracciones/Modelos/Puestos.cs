namespace Abstracciones.Modelos
{
    public class Puestos
    {
        public string Nombre { get; set; }
        public string Descripcion{ get; set; }
        public int EstadoId { get; set; }
    }
    public class PuestosResponse:Puestos
    {
        public Guid PuestosId { get; set; }
    }
}
