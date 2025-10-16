using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class Empleado
    {
        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(9, ErrorMessage = "La cédula no puede tener más de 9 caracteres")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(150, ErrorMessage = "El apellido no puede exceder los 150 caracteres")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El telefono  es requerido")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo electrónico no es válido")]
        public string CorreoElectronico { get; set; }

        [Required(ErrorMessage = "El puesto es obligatorio")]
        
        public Guid PuestoId { get; set; }

        [Required(ErrorMessage = "Debe indicar padecimientos (si no tiene, escribir 'Ninguno')")]
        [StringLength(250, ErrorMessage = "Los padecimientos no pueden exceder los 250 caracteres")]
        public string Padecimientos { get; set; }

        [Required(ErrorMessage = "La cuenta bancaria es obligatoria")]
        [RegularExpression(@"^\d{6,20}$", ErrorMessage = "La cuenta bancaria debe contener solo números (entre 6 y 20 dígitos)")]
        public string CuentaBancaria { get; set; }

        

        [Required(ErrorMessage = "El horario es obligatorio")]
       
        public Guid HorarioId { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
        public DateTime FechaIngreso { get; set; }
        [Required(ErrorMessage = "La fecha de salida es obligatoria")]
        public DateTime FechaSalida { get; set; }

    }
    public class EmpleadoResponse: EmpleadoPlanilla
    {
        public Guid IdEmpleado { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public int EstadoId { get; set; }
        public string  Horario{ get; set; }
        public string Puesto { get; set; }
        public int? Cantidadausencias { get; set; }

    }
    public class EmpleadoPlanilla : Empleado
    {
        public double Sueldo { get; set; }
        
        
        
        public string banco{ get; set; }
        public string tipoCuenta{ get; set; }
        /*
        public string tipoAusencia { get; set; }
        public string MotivoAusencia{ get; set; }
        public bool aprobadoAusencia{ get; set; }
        public DateOnly fechaInicioAusencia { get; set; }
        public DateOnly fechaFinAusencia { get; set; }
        public string TipoIncapacidad { get; set; }
        public string InstitucionMedicaIncapacidad { get; set; }
        public DateOnly fechaInicioIncapacidad { get; set; }
        public DateOnly fechaFinIncapacidad { get; set; }
        */ //esto es para el otro modelo de ausencias e incapacidades



    }
}
