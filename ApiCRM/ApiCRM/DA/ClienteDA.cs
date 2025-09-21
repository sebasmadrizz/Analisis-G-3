using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;


namespace DA
{
	public class ClienteDA: IClienteDA
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
				CLIENTE_ID = Guid.NewGuid(),
				TIPO_CLIENTE = cliente.TIPO_CLIENTE,
				NOMBRE = cliente.NOMBRE,
				IDENTIFICACION = cliente.IDENTIFICACION,
				CORREO = cliente.CORREO,
				TELEFONO = cliente.TELEFONO,
				DIRECCION = cliente.DIRECCION,
				FECHA_CREACION = DateTime.Now,
				FECHA_ACTUALIZACION = cliente.FECHA_ACTUALIZACION,
				ESTADO_ID = 1

			});
			return resultadoConsulta;
		}


		public async Task<Guid> Editar(Guid CLIENTE_ID, Cliente cliente)
		{
			await VerificarExistenciaCliente(CLIENTE_ID);


			string query = @"EDITAR_CLIENTE";

			var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
				CLIENTE_ID = CLIENTE_ID,          
				TIPO_CLIENTE = cliente.TIPO_CLIENTE,
				NOMBRE = cliente.NOMBRE,
				IDENTIFICACION = cliente.IDENTIFICACION,
				CORREO = cliente.CORREO,
				TELEFONO = cliente.TELEFONO,
				DIRECCION = cliente.DIRECCION
			});

			return resultado;
		}

		public async Task<Guid> Eliminar(Guid CLIENTE_ID)
		{
			await VerificarExistenciaCliente(CLIENTE_ID);
			string query = @"ESTADO_CLIENTE";
			var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
				CLIENTE_ID = CLIENTE_ID
			});
			return resultadoConsulta;
		}

		public async Task<IEnumerable<ClienteResponse>> Obtener()
		{
			string query = @"VER_CLIENTES";
			var resultadoConsulta = await _sqlConnection.QueryAsync<ClienteResponse>(query);
			return resultadoConsulta;
		}

		public async Task<ClienteResponse> ObtenerPorId(Guid CLIENTE_ID)
		{
			string query = @"VER_CLIENTE_POR_ID";
			var resultadoConsulta = await _sqlConnection.QueryAsync<ClienteResponse>(query,
				new { CLIENTE_ID = CLIENTE_ID });
			return resultadoConsulta.FirstOrDefault();
		}
		private async Task VerificarExistenciaCliente(Guid CLIENTE_ID)
		{
			ClienteResponse? resutadoConsulta = await ObtenerPorId(CLIENTE_ID);
			if (resutadoConsulta == null)
				throw new Exception("no se encontro el cliente");
		}
	}
}
