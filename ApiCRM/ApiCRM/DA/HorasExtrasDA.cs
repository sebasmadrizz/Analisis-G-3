using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class HorasExtrasDA: IHorasExtrasDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        public HorasExtrasDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(HorasExtras horasExtras)
        {
            string query = @"AGREGAR_HORAS_EXTRAS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                HorasExtrasId = Guid.NewGuid(),
                EmpleadoId=horasExtras.EmpleadoId,
                FechaRealizacion = horasExtras.FechaRealizacion,
                CantidadHoras= horasExtras.CantidadHoras,
                TarifaHora = horasExtras.TarifaHora,
                Descripcion = horasExtras.Descripcion,
                EstadoId = 1,
                ProcesadoPagoId = horasExtras.ProcesadoPagoId,

            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid HorasExtrasId, HorasExtras horasExtras)
        {
            //await VerificarExistenciaEmpleado(IdEmpleado);
            string query = @"EDITAR_HORAS_EXTRAS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                HorasExtrasId = HorasExtrasId,
                FechaRealizacion = horasExtras.FechaRealizacion,
                CantidadHoras = horasExtras.CantidadHoras,
                TarifaHora = horasExtras.TarifaHora,
                Descripcion = horasExtras.Descripcion,
                ProcesadoPagoId = horasExtras.ProcesadoPagoId,

            });
            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid HorasExtrasId)
        {
            //await VerificarExistenciaEmpleado(IdEmpleado);
            string query = @"ESTADO_HorasExtras";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                HorasExtrasId = HorasExtrasId
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<HorasExtrasResponse>> Obtener()
        {
            string query = @"VER_HORAS_EXTRAS";
            var resultadoConsulta = await _sqlConnection.QueryAsync<HorasExtrasResponse>(query);
            return resultadoConsulta;
        }

        public async Task<HorasExtrasResponse> ObtenerPorId(Guid HorasExtrasId)
        {
            string query = @"VER_HORAS_EXTRAS_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<HorasExtrasResponse>(query,
                new { HorasExtrasId = HorasExtrasId });
            return resultadoConsulta.FirstOrDefault();
        }
    }
}
