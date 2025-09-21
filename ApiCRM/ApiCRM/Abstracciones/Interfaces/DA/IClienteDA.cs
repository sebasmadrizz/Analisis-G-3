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
		Task<ClienteResponse> ObtenerPorId(Guid CLIENTE_ID);
		Task<Guid> Agregar(Cliente cliente);

		Task<Guid> Editar(Guid CLIENTE_ID, Cliente cliente);

		Task<Guid> Eliminar(Guid CLIENTE_ID);
	}
}
