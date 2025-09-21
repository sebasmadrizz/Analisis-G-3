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

		public async Task<Guid> Editar(Guid CLIENTE_ID, Cliente cliente)
		{
			return await _clienteDA.Editar(CLIENTE_ID, cliente);
		}

		public async Task<Guid> Eliminar(Guid CLIENTE_ID)
		{
			return await _clienteDA.Eliminar(CLIENTE_ID);
		}

		public async Task<IEnumerable<ClienteResponse>> Obtener()
		{
			return await _clienteDA.Obtener();
		}

		public async Task<ClienteResponse> ObtenerPorId(Guid CLIENTE_ID)
		{
			return await _clienteDA.ObtenerPorId(CLIENTE_ID);
		}
	}
}

