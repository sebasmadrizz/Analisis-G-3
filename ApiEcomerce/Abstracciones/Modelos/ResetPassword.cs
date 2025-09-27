using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class ResetPassword
    {
        
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
       
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        
        public string Password { get; set; } 

        [Compare("Password", ErrorMessage = "La nueva contraseña y la confirmación no son iguales.")]
        [DataType(DataType.Password)]
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        
        public string ConfirmPassword { get; set; } 

        public string Token { get; set; }
        


    }
    public class ResetPasswordToken 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string TokenHash { get; set; }
        public DateTime ExpiraEn { get; set; }
        public bool Usado{ get; set; }


    }
}
