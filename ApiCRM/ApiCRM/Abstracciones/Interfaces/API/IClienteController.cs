
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
	public interface IClienteController
	{
		Task<IActionResult> Obtener();
		Task<IActionResult> ObtenerPorId(Guid CLIENTE_ID);

		Task<IActionResult> Agregar(Cliente cliente);
		Task<IActionResult> Editar(Guid CLIENTE_ID, Cliente cliente);
		Task<IActionResult> Eliminar(Guid CLIENTE_ID);
	}
}
