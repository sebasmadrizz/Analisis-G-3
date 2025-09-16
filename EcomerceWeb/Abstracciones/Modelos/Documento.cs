namespace Abstracciones.Modelos
{
    public class Documento
    {
        public Guid Id { get; set; }
        public byte[] Contenido { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
    }
}
