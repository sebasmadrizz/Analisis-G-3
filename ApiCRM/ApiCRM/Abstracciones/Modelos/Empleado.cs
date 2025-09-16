using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class Empleado
    {
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(9, ErrorMessage = "La cédula no puede tener más de 9 caracteres")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
        public string NombreCompleto { get; set; }
        [Required(ErrorMessage = "El telefono  es requerido")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo electrónico no es válido")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El puesto es obligatorio")]
        [StringLength(100, ErrorMessage = "El puesto no puede exceder los 100 caracteres")]
        public string Puesto { get; set; }

        [Required(ErrorMessage = "Debe indicar padecimientos (si no tiene, escribir 'Ninguno')")]
        [StringLength(250, ErrorMessage = "Los padecimientos no pueden exceder los 250 caracteres")]
        public string Padecimientos { get; set; }

        [Required(ErrorMessage = "La cuenta bancaria es obligatoria")]
        [RegularExpression(@"^\d{6,20}$", ErrorMessage = "La cuenta bancaria debe contener solo números (entre 6 y 20 dígitos)")]
        public string CuentaBancaria { get; set; }

        [Required(ErrorMessage = "El tipo de contrato es obligatorio")]
        [StringLength(50)]
        public string TipoContrato { get; set; }

        [Required(ErrorMessage = "La jornada es obligatoria")]
        [StringLength(50)]
        public string Jornada { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
        public DateTime FechaIngreso { get; set; }
  
    }
    public class EmpleadoResponse:Empleado
    {
        public Guid IdEmpleado { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public int EstadoId { get; set; }

    }
}
