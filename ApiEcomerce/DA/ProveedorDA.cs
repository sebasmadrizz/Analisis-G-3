using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ProveedorDA: IProveedorDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ProveedorDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(Proveedores proveedor)
        {
            string query = @"AGREGAR_PROVEEDOR";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                ProveedorId = proveedor.PROVEEDOR_ID,
                Nombre = proveedor.Nombre_PROVEEDOR,
                CorreoElectronico = proveedor.Correo_ELECTRONICO,
                Tipo = proveedor.TIPO,
                Direccion = proveedor.Direccion,
                Telefono = proveedor.Telefono,
                EstadoId = proveedor.ESTADO_ID,
                FechaCreacion = proveedor.Fecha_Registro,
                NombreContacto = proveedor.Nombre_Contacto

            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<Proveedores>> Obtener()
        {
            string query = @"VER_PROVEEDORES";
            var resultadoConsulta = await _sqlConnection.QueryAsync<Proveedores>(query);
            return resultadoConsulta;
        }

        public async Task<Proveedores> ObtenerPorId(Guid IdProveedor)
        {
            string query = @"VER_PROVEEDOR_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<Proveedores>(query,
                new { IdProveedor = IdProveedor });
            return resultadoConsulta.FirstOrDefault();
        }

		public async Task<Guid> Editar(Guid IdProveedor, Proveedores proveedor)
		{
			await VerificarExistenciaProveedor(IdProveedor);


			string query = @"EDITAR_PROVEEDOR";

			var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                ProveedorId = IdProveedor,
                Nombre = proveedor.Nombre_PROVEEDOR,
				CorreoElectronico = proveedor.Correo_ELECTRONICO,
				Telefono = proveedor.Telefono,
				Direccion = proveedor.Direccion,
				NombreContacto = proveedor.Nombre_Contacto,
				FechaRegistro = proveedor.Fecha_Registro,
				Tipo = proveedor.TIPO,
				EstadoId = proveedor.ESTADO_ID
			});

			return resultado;
		}

		public async Task<Guid> Eliminar(Guid IdProveedor)
		{
			await VerificarExistenciaProveedor(IdProveedor);
			string query = @"ESTADO_PROVEEDOR";
			var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
                IdProveedor = IdProveedor
            });
			return resultadoConsulta;
		}

		private async Task VerificarExistenciaProveedor(Guid IdProveedor)
		{
			Proveedores? resutadoConsultaProducto = await ObtenerPorId(IdProveedor);
			if (resutadoConsultaProducto == null)
				throw new Exception("no se encontro el producto");
		}
	}
}
