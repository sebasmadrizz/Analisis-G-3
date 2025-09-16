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
using static Abstracciones.Modelos.Categorias;

namespace DA
{
	public class CategoriasDA : ICategoriasDA
	{

		private IRepositorioDapper _repositorioDapper;
		private SqlConnection _sqlConnection;


		public CategoriasDA(IRepositorioDapper repositorioDapper)
		{
			_repositorioDapper = repositorioDapper;
			_sqlConnection = _repositorioDapper.ObtenerRepositorio();
		}

        public async Task<Guid> ActivarHijas(Guid idCategoria)
        {
            await VerificarExistenciaCategoria(idCategoria);


            string query = @"ACTIVAR_HIJA_Y_PADRE";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdHija = idCategoria
             
            });

            return resultado;
        }

        public async Task<Guid> ActivarPadreHijas(Guid idCategoria, bool activarHijas)
        {
            await VerificarExistenciaCategoria(idCategoria);


            string query = @"ACTIVAR_PADRE_Y_HIJAS";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdCategoria = idCategoria,
                ActivarHijas = activarHijas,
            });

            return resultado;
        }

        public async Task<Guid> AgregarHija(CategoriasRequestHija categorias)
        {
            string query = @"AGREGAR_CATEGORIA_HIJA";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {

                IdCategoria = Guid.NewGuid(),
                IdCategoriaPadre = categorias.PadreId,
                Nombre = categorias.Nombre,
                Descripcion = categorias.Descripcion,
                FechaCreacion = DateTime.Now,
                EstadoId = 1

            });
            return resultadoConsulta;
        }

        public async Task<Guid> AgregarPadre(CategoriasRequestPadre categorias)
        {
            string query = @"AGREGAR_CATEGORIA_PADRE";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {

                IdCategoria = Guid.NewGuid(),
                Nombre = categorias.Nombre,
                Descripcion = categorias.Descripcion,
                FechaCreacion = DateTime.Now,
                EstadoId = 1

            });
            return resultadoConsulta;
        }

        public async Task<Guid> Desactivar(Guid IdCategoria)
        {
            await VerificarExistenciaCategoria(IdCategoria);


            string query = @"DESACTIVAR_CATEGORIA";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdCategoria = IdCategoria,
                EstadoId = 2,
            });

            return resultado;
        }

        public async Task<Guid> Editar(Guid IdCategoria, CategoriasRequestPadre categorias)
		{
			await VerificarExistenciaCategoria(IdCategoria);


			string query = @"EDITAR_CATEGORIA";

			var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
			{
                IdCategoria = IdCategoria,
                Nombre = categorias.Nombre,
                Descripcion = categorias.Descripcion
            });

			return resultado;
		}

        
        public async Task<IEnumerable<CategoriasResponse>> Obtener()
		{
			string query = @"VER_CATEGORIAS";
			var resultadoConsulta = await _sqlConnection.QueryAsync<CategoriasResponse>(query);
			return resultadoConsulta;
		}

        public async Task<IEnumerable<CategoriasResponse>> ObtenerHijas(Guid idPadre)
        {
            return await _sqlConnection.QueryAsync<CategoriasResponse>(
                "OBTENER_CATEGORIAS_HIJAS",
                new { IdPadre = idPadre },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<CategoriasResponse>> ObtenerHijasRecursivo(Guid idPadre)
        {
            return await _sqlConnection.QueryAsync<CategoriasResponse>(
             "OBTENER_CATEGORIAS_RECURSIVO",
             new { IdPadre = idPadre },
             commandType: System.Data.CommandType.StoredProcedure
         );
        }

        public async Task<VerificarCategoriaResponse> ObtenerHijasTotales(Guid IdCategoria)
        {
            string query = @"CONTAR_HIJAS_TOTALES";
            var resultadoConsulta = await _sqlConnection.QueryAsync<VerificarCategoriaResponse>(query,
                new { IdCategoria = IdCategoria });
            return resultadoConsulta.FirstOrDefault();
        }

        public async Task<IEnumerable<CategoriasResponse>> ObtenerPadres()
        {
            string query = @"VER_CATEGORIAS_PADRES";
            var resultadoConsulta = await _sqlConnection.QueryAsync<CategoriasResponse>(query);
            return resultadoConsulta;
        }

        public async Task<CategoriasResponse> ObtenerPorId(Guid IdCategoria)
		{
			string query = @"VER_CATEGORIA_POR_ID";
			var resultadoConsulta = await _sqlConnection.QueryAsync<CategoriasResponse>(query,
				new { IdCategoria = IdCategoria });
			return resultadoConsulta.FirstOrDefault();
		}

        public async  Task<int> TieneHijas(Guid IdCategoria)
        {
            var count = await _sqlConnection.ExecuteScalarAsync<int>(
       "CONTAR_HIJAS_ACTIVAS",
       new { IdCategoria = IdCategoria },
       commandType: CommandType.StoredProcedure
   );
            return count;
        }

        private async Task VerificarExistenciaCategoria(Guid IdCategoria)
		{
			CategoriasResponse? resutadoConsultaProducto = await ObtenerPorId(IdCategoria);
			if (resutadoConsultaProducto == null)
				throw new Exception("la categoria no esta registrada");
		}

	}
}
