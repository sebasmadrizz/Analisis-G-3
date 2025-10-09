using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class CambiarContraseña
    {
        [Required]
        public string ContraseñaActual { get; set; }
        [Required]
        public string NuevaContraseña { get; set; }
        [Required]
        public string Correo { get; set; }
        




    }
}
