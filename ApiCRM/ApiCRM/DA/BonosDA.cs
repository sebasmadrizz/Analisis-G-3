using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class BonosDA : IBonosDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        public BonosDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }
        public async Task<Guid> Agregar(Bonos bonos)
        {
            string query = @"AGREGAR_BONOS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                BonosId = Guid.NewGuid(),
                EmpleadoId = bonos.EmpleadoId,
                Monto = bonos.Monto,
                FechaAsignacion = bonos.FechaAsignacion,
                Descripcion = bonos.Descripcion,
                EstadoId = 1,
                ProcesadoPagoId = bonos.ProcesadoPagoId,


            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid BonosId, Bonos bonos)
        {
            //validaciones
            string query = @"EDITAR_BONOS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                BonosId = BonosId,
                Monto = bonos.Monto,
                FechaAsignacion = bonos.FechaAsignacion,
                Descripcion = bonos.Descripcion,
                ProcesadoPagoId = bonos.ProcesadoPagoId,
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid BonosId)
        {
            //await VerificarExistenciaEmpleado(IdEmpleado);
            string query = @"ESTADO_BONOS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                BonosId = BonosId
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<BonosResponse>> Obtener()
        {
            string query = @"VER_BONOS";
            var resultadoConsulta = await _sqlConnection.QueryAsync<BonosResponse>(query);
            return resultadoConsulta;
        }

        public async Task<BonosResponse> ObtenerPorId(Guid BonosId)
        {
            string query = @"VER_BONOS_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<BonosResponse>(query,
                new { BonosId = BonosId });
            return resultadoConsulta.FirstOrDefault();
        }
    }
}
