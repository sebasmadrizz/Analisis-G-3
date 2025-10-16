using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class AusenciasDA: IAusenciasDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        public AusenciasDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(Ausencias ausencias)
        {
            string query = @"AGREGAR_AUSENCIAS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                AusenciaId = Guid.NewGuid(),
                IdEmpleado = ausencias.IdEmpleado,
                FechaInicio = ausencias.FechaInicio,
                FechaFin = ausencias.FechaFin,
                Motivo = ausencias.Motivo,
                Aprobada = ausencias.Aprobada,
                FechaRegistro = DateTime.Now,
                TipoAusencia= ausencias.TipoAusencia


            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid AusenciasId, Ausencias ausencias)
        {
            //validaciones
            string query = @"EDITAR_AUSENCIAS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                AusenciaId = AusenciasId,
                FechaInicio = ausencias.FechaInicio,
                FechaFin = ausencias.FechaFin,
                Motivo = ausencias.Motivo,
                Aprobada = ausencias.Aprobada,
                TipoAusencia = ausencias.TipoAusencia


            });
            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid AusenciasId)
        {
            //await VerificarExistenciaEmpleado(IdEmpleado);
            string query = @"ESTADO_AUSENCIAS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                AusenciasId = AusenciasId
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<AusenciasResponse>> Obtener()
        {
            string query = @"VER_AUSENCIAS";
            var resultadoConsulta = await _sqlConnection.QueryAsync<AusenciasResponse>(query);
            return resultadoConsulta;
        }

        public async Task<AusenciasResponse> ObtenerPorId(Guid AusenciasId)
        {
            string query = @"VER_AUSENCIAS_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<AusenciasResponse>(query,
                new { AusenciasId = AusenciasId });
            return resultadoConsulta.FirstOrDefault();
        }
    }
}
