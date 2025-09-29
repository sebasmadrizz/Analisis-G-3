using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
	public interface IClienteFlujo
	{
		Task<IEnumerable<ClienteResponse>> Obtener();
		Task<ClienteResponse> ObtenerPorId(Guid ClienteId);
		Task<Guid> Agregar(Cliente cliente);
		Task<Guid> Editar(Guid ClienteId, Cliente cliente);
		Task<Guid> Eliminar(Guid ClienteId);
	}
}

