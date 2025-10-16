using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class PuestosDA: IPuestosDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        public PuestosDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(Puestos puestos)
        {
            string query = @"AGREGAR_PUESTOS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                PuestosId = Guid.NewGuid(),
                Nombre = puestos.Nombre,
                Descripcion = puestos.Descripcion,
                EstadoId = 1
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid PuestosId, Puestos puestos)
        {
            //validaciones
            string query = @"EDITAR_PUESTOS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                PuestosId = PuestosId,
                Nombre = puestos.Nombre,
                Descripcion = puestos.Descripcion,
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid PuestosId)
        {
            string query = @"ESTADO_PUESTOS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                PuestosId = PuestosId
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<PuestosResponse>> Obtener()
        {
            string query = @"VER_PUESTOS";
            var resultadoConsulta = await _sqlConnection.QueryAsync<PuestosResponse>(query);
            return resultadoConsulta;
        }

        public async Task<PuestosResponse> ObtenerPorId(Guid PuestosId)
        {
            string query = @"VER_PUESTOS_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<PuestosResponse>(query,
                new { PuestosId = PuestosId });
            return resultadoConsulta.FirstOrDefault();
        }
    }
}
