
using System.ComponentModel.DataAnnotations;
using Abstracciones.Modelos.Clientes;

namespace Abstracciones.Modelos.Clientes
{
	public class Cliente
	{
		public Guid ClienteId { get; set; }
		[Required(ErrorMessage = "El tipo de cliente es obligatorio")]
		[StringLength(50)]
		public string TipoCliente { get; set; }

		[Required(ErrorMessage = "El nombre completo es obligatorio")]
		[StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres")]
		public string Nombre { get; set; }

		[Required(ErrorMessage = "La cédula es obligatoria")]
		[StringLength(9, ErrorMessage = "La cédula no puede tener más de 9 caracteres")]
		public string Identificacion { get; set; }

		[Required(ErrorMessage = "El correo electrónico es obligatorio")]
		[EmailAddress(ErrorMessage = "Formato de correo inválido")]
		[RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo electrónico no es válido")]
		public string Correo { get; set; }

		[Required(ErrorMessage = "El teléfono es requerido")]
		[RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos")]
		public string Telefono { get; set; }

		[Required(ErrorMessage = "La dirección es requerida")]
		[RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s,.\-]+$", ErrorMessage = "Solo se permiten letras y espacios")]
		public string Direccion { get; set; }
		public DateTime FechaCreacion { get; set; }
		public DateTime FechaActualizacion { get; set; }
		public int EstadoId { get; set; }
	}

	public class ClienteResponse : Cliente
	{
		public Guid ClienteId { get; set; }

		public int EstadoId { get; set; }
	}
}

public class ClienteRequest : Cliente
{
	[Required]
	public Guid ClienteId { get; set; }
}