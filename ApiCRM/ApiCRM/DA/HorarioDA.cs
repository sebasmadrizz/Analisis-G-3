using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class HorarioDA: IHorarioDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        public HorarioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(Horario horario)
        {
            string query = @"AGREGAR_HORARIO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                HorarioId = Guid.NewGuid(),
                Nombre = horario.Nombre,
                Entrada = horario.Entrada,
                Salida = horario.Salida,
                EstadoId = 1
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid HorarioId, Horario horario)
        {
            //validaciones
            string query = @"EDITAR_HORARIO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                HorarioId = HorarioId,
                Nombre = horario.Nombre,
                Entrada = horario.Entrada,
                Salida = horario.Salida
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid HorarioId)
        {
            string query = @"ESTADO_HORARIO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                HorarioId = HorarioId
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<HorarioResponse>> Obtener()
        {
            string query = @"VER_HORARIO";
            var resultadoConsulta = await _sqlConnection.QueryAsync<HorarioResponse>(query);
            return resultadoConsulta;
        }

        public async Task<HorarioResponse> ObtenerPorId(Guid HorarioId)
        {
            string query = @"VER_HORARIO_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<HorarioResponse>(query,
                new { HorarioId = HorarioId });
            return resultadoConsulta.FirstOrDefault();
        }
    }
}
