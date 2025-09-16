using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using static Abstracciones.Modelos.CarritoProducto;

namespace DA
{
	public class CarritoProductoDA : ICarritoProductoDA
	{
		private IRepositorioDapper _repositorioDapper;
		private SqlConnection _sqlConnection;


		public CarritoProductoDA(IRepositorioDapper repositorioDapper)
		{
			_repositorioDapper = repositorioDapper;
			_sqlConnection = _repositorioDapper.ObtenerRepositorio();
		}


		public async Task<Guid> Agregar(CarritoProductoRequest carritoProducto)
		{
			string query = @"AGREGAR_CARRITOPRODUCTO";
			var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{

				CarritoProductoId = Guid.NewGuid(),
				CarritoId = carritoProducto.CarritoId,
				ProductosId = carritoProducto.ProductosId,
				Cantidad = carritoProducto.Cantidad,
			});
			return resultadoConsulta;
		}

        public async Task DescontarStock(Guid productoId, int cantidadSolicitada)
        {
            string query = "DESCONTAR_STOCK";

            await _sqlConnection.ExecuteAsync(
                query,
                new
                {
                    ProductoId = productoId,
                    Cantidad= cantidadSolicitada
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DevolverStock(Guid ProductoId, int cantidad)
        {
            string query = "DEVOLVER_STOCK";

            await _sqlConnection.ExecuteAsync(
                query,
                new
                {
                    ProductoId = ProductoId,
                    Cantidad = cantidad
                },
                commandType: CommandType.StoredProcedure
            );
        }


        public async Task<Guid> Editar(Guid CarritoProductoId, CarritoProductoRequest carritoProducto)
		{

			string query = @"EDITAR_CARRITOPRODUCTO";

			var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
				CarritoId = carritoProducto.CarritoId,
				CarritoProductoId = CarritoProductoId,
				Cantidad = carritoProducto.Cantidad,
				ProductosId = carritoProducto.ProductosId,
			});

			return resultado;
		}


		public async Task<Guid> Eliminar(Guid CarritoProductoId)
		{

			string query = @"ELIMINAR_CARRITOPRODUCTO";
			var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
				CarritoProductoId = CarritoProductoId
			});
			return resultadoConsulta;
		}

		public async Task<List<CarritoProductoResponse>> ObtenerPorCarrito(Guid CarritoId)
		{
			string query = "VER_CARRITOPRODUCTOS_POR_CARRITO";
			var resultadoConsulta = await _sqlConnection.QueryAsync<CarritoProductoResponse>(
				query,
				new { CarritoId = CarritoId }

			);
			return resultadoConsulta.ToList();
		}


		public async Task<CarritoProductoResponse> ObtenerPorID(Guid CarritoProductoId)
		{
			string query = "VER_CARRITOPRODUCTO_POR_ID";

			var resultadoConsulta = await _sqlConnection.QueryAsync<CarritoProductoResponse>(
				query,
				new { CarritoProductoId = CarritoProductoId },
				commandType: CommandType.StoredProcedure
			);

			return resultadoConsulta.FirstOrDefault();
		}

		public async Task<bool> ValidarStock( Guid ProductoId, int cantidadSolicitada)
		{
			string query = "VALIDAR_STOCK_PRODUCTO";

			var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<bool>(
				query,
				new
				{
					
					ProductoId = ProductoId,
					CantidadSolicitada = cantidadSolicitada
				});
			return resultadoConsulta;
		}
	}
}
