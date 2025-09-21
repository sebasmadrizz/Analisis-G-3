using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
	public class Cliente{

		[Required(ErrorMessage = "El tipo de cliente es obligatorio")]
		[StringLength(50)]
		public string TIPO_CLIENTE { get; set; }

		[Required(ErrorMessage = "El nombre completo es obligatorio")]
		[StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
		public string NOMBRE { get; set; }

		[Required(ErrorMessage = "La cédula es obligatoria")]
		[StringLength(9, ErrorMessage = "La cédula no puede tener más de 9 caracteres")]
		public string IDENTIFICACION { get; set; }

		[Required(ErrorMessage = "El correo electrónico es obligatorio")]
		[EmailAddress(ErrorMessage = "Formato de correo inválido")]
		[RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo electrónico no es válido")]
		public string CORREO { get; set; }


		[Required(ErrorMessage = "El telefono  es requerido")]
		[RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos")]
		public string TELEFONO { get; set; }

		[Required(ErrorMessage = "La direccion es requerida")]
		[RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s,.\-]+$", ErrorMessage = "Solo se permiten letras y espacios")]
		public string DIRECCION { get; set; }

		public DateTime FECHA_ACTUALIZACION { get; set; }




	}
	public class ClienteResponse :Cliente
	{
		public Guid CLIENTE_ID { get; set; }
		//public string Estado { get; set; }
		public DateTime FECHA_CREACION { get; set; }
		public DateTime FECHA_ACTUALIZACION { get; set; }
		public int ESTADO_ID { get; set; }

	}

}
