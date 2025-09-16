using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class Usuario
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        
        public string NombreUsuario { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "El correo electronico es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electronico valido")]
        public string CorreoElectronico { get; set; }

        [Required]
        public int IdEstado { get; set; }
        [Required(ErrorMessage = "El telefono  es requerido")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "La direccion es requerida")]
        
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El apellido es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
        public string Apellido { get; set; }

    }
    public class UsuarioEditar
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        
        public string NombreUsuario { get; set; }
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
}
