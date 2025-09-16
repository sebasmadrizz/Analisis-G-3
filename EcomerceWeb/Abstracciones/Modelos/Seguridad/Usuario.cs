using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos.Seguridad
{
    public class UsuarioBase
    {

        [Required(ErrorMessage = "El nombre es requerido")]
        
        public string NombreUsuario { get; set; }

        public Guid IdUsuario { get; set; }
        public string? PasswordHash { get; set; }
        [Required(ErrorMessage = "El correo electronico es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electronico valido")]
        public string CorreoElectronico { get; set; }
        [Required(ErrorMessage = "El telefono  es requerido")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos")]

        public string Telefono { get; set; }
        [Required(ErrorMessage = "La direccion es requerida")]
        
        public string Direccion { get; set; }
        
        [Required(ErrorMessage = "El apellido es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
        public string Apellido { get; set; }


    }
    public class Usuario : UsuarioBase
    {
        [Required]
        public string Password { get; set; }
        
    }
}
