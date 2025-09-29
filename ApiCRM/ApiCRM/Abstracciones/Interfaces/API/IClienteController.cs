
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
	public interface IClienteController
	{
		Task<IActionResult> Obtener();
		Task<IActionResult> ObtenerPorId(Guid ClienteId);

		Task<IActionResult> Agregar(Cliente cliente);
		Task<IActionResult> Editar(Guid ClienteId, Cliente cliente);
		Task<IActionResult> Eliminar(Guid ClienteId);
	}
}
