using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DA
{
	public class ClienteDA : IClienteDA
	{
		private IRepositorioDapper _repositorioDapper;
		private SqlConnection _sqlConnection;
		public ClienteDA(IRepositorioDapper repositorioDapper)
		{
			_repositorioDapper = repositorioDapper;
			_sqlConnection = _repositorioDapper.ObtenerRepositorio();
		}

		public async Task<Guid> Agregar(Cliente cliente)
		{
			string query = @"AGREGAR_CLIENTE";
			var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
				ClienteId = Guid.NewGuid(),
				TipoCliente = cliente.TipoCliente,
				Nombre = cliente.Nombre,
				Identificacion = cliente.Identificacion,
				Correo = cliente.Correo,
				Telefono = cliente.Telefono,
				Direccion = cliente.Direccion,
				FechaCreacion = DateTime.Now,
				FechaActualizacion = cliente.FechaActualizacion,
				EstadoId = 1
			});
			return resultadoConsulta;
		}


		public async Task<Guid> Editar(Guid ClienteId, Cliente cliente)
		{
			await VerificarExistenciaCliente(ClienteId);


			string query = @"EDITAR_CLIENTE";

			var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
				ClienteId = ClienteId,
				TipoCliente = cliente.TipoCliente,
				Nombre = cliente.Nombre,
				Identificacion = cliente.Identificacion,
				Correo = cliente.Correo,
				Telefono = cliente.Telefono,
				Direccion = cliente.Direccion
			});

			return resultado;
		}

		public async Task<Guid> Eliminar(Guid ClienteId)
		{
			await VerificarExistenciaCliente(ClienteId);
			string query = @"ESTADO_CLIENTE";
			var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
				ClienteId = ClienteId
			});
			return resultadoConsulta;
		}

		public async Task<IEnumerable<ClienteResponse>> Obtener()
		{
			string query = @"VER_CLIENTES";
			var resultadoConsulta = await _sqlConnection.QueryAsync<ClienteResponse>(query);
			return resultadoConsulta;
		}

		public async Task<ClienteResponse> ObtenerPorId(Guid ClienteId)
		{
			string query = @"VER_CLIENTE_POR_ID";
			var resultadoConsulta = await _sqlConnection.QueryAsync<ClienteResponse>(query,
				new { ClienteId = ClienteId });
			return resultadoConsulta.FirstOrDefault();
		}
		private async Task VerificarExistenciaCliente(Guid ClienteId)
		{
			ClienteResponse? resutadoConsulta = await ObtenerPorId(ClienteId);
			if (resutadoConsulta == null)
				throw new Exception("no se encontro el cliente");
		}
	}
}
