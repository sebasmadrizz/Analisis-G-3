using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
	public interface IClienteDA
	{
		Task<IEnumerable<ClienteResponse>> Obtener();
		Task<ClienteResponse> ObtenerPorId(Guid ClienteId);
		Task<Guid> Agregar(Cliente cliente);

		Task<Guid> Editar(Guid ClienteId, Cliente cliente);

		Task<Guid> Eliminar(Guid ClienteId);
	}
}
