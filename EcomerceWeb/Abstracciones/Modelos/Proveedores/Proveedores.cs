using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos.Proveedores
{
    public class ProveedoresBase
    {
		public Guid PROVEEDOR_ID { get; set; }
		[Required(ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
		[Display(Name = "Nombre del proveedor")]
		public string Nombre_PROVEEDOR { get; set; }
        [Required(ErrorMessage = "El correo electronico es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electronico valido")]

		[Display(Name = "Correo electonico")]
		public string Correo_ELECTRONICO { get; set; }
        [Required(ErrorMessage = "El tipo es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
		[Display(Name = "Tipo de productos")]
		public string TIPO { get; set; }
        [Required(ErrorMessage = "La direccion es requerida")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
		[Display(Name = "Dirección")]
        public string Direccion { get; set; }

		[Display(Name = "Teléfono")]
		[Required(ErrorMessage = "El telefono  es requerido")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos")]
        public string Telefono { get; set; }
        [Required]
        public int ESTADO_ID { get; set; }
        [Required]
        public DateTime Fecha_Registro { get; set; }
        [Required(ErrorMessage = "El nombre de contacto es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
		[Display(Name = "Nombre de contacto ")]
		public string Nombre_Contacto { get; set; }

    }
	public class ProveedoresRequest : ProveedoresBase
	{
		[Required]
		public Guid PROVEEDOR_ID { get; set; }
	}

	public class ProveedoresResponse: ProveedoresBase
    {
        [Required]
        public Guid PROVEEDOR_ID { get; set; }
    }
}
