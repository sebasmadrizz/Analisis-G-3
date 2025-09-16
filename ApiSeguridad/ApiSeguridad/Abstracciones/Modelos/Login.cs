using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class Login
    {
        [Required]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
        public string NombreUsuario { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "El correo electronico es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electronico valido")]
        public string CorreoElectronico { get; set; }
        public Guid IdServicio { get; set; }
    }
}
