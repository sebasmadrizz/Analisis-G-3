using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.Seguridad
{
    public class CambiarContrasenaInfo
    {
        [Required]
        public string ContraseñaActual { get; set; }
        public string NuevaContraseñaHash { get; set; }
        [Required]
        public string NuevaContraseña { get; set; }
        [Required]
        public string Correo { get; set; }
        [Required]
        public string ConfirmarContrasena { get; set; }
    }
    public class CambiarContrasenaRequest
    {
        [Required]
        public string ContraseñaActual { get; set; }
        [Required]
        public string NuevaContraseña { get; set; }
        [Required]
        public string Correo { get; set; }
    }

}
