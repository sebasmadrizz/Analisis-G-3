using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
	public class ClienteFlujo : IClienteFlujo
	{
		private readonly IClienteDA _clienteDA;
		public ClienteFlujo(IClienteDA clienteDA)
		{
			_clienteDA = clienteDA;
		}
		public async Task<Guid> Agregar(Cliente cliente)
		{
			return await _clienteDA.Agregar(cliente);
		}

		public async Task<Guid> Editar(Guid ClienteId, Cliente cliente)
		{
			return await _clienteDA.Editar(ClienteId, cliente);
		}

		public async Task<Guid> Eliminar(Guid ClienteId)
		{
			return await _clienteDA.Eliminar(ClienteId);
		}

		public async Task<IEnumerable<ClienteResponse>> Obtener()
		{
			return await _clienteDA.Obtener();
		}

		public async Task<ClienteResponse> ObtenerPorId(Guid ClienteId)
		{
			return await _clienteDA.ObtenerPorId(ClienteId);
		}
	}
}

