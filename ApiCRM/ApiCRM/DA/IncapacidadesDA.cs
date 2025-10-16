using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class IncapacidadesDA: IIncapacidadesDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        public IncapacidadesDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(Incapacidades incapacidad)
        {
            string query = @"AGREGAR_AUSENCIAS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IncapacidadId = Guid.NewGuid(),
                EmpleadoId = incapacidad.EmpleadoId,
                TipoIncapacidad = incapacidad.TipoIncapacidad,
                FechaInicio = incapacidad.FechaInicio,
                FechaFin = incapacidad.FechaFin,
                InstitucionMedica = incapacidad.InstitucionMedica,
                FechaRegistro = DateTime.Now
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid IncapacidadesId, Incapacidades incapacidad)
        {
            //validaciones
            string query = @"EDITAR_AUSENCIAS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IncapacidadId = IncapacidadesId,
                TipoIncapacidad = incapacidad.TipoIncapacidad,
                FechaInicio = incapacidad.FechaInicio,
                FechaFin = incapacidad.FechaFin,
                InstitucionMedica = incapacidad.InstitucionMedica,
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Eliminar(Guid IncapacidadesId)
        {
            //await VerificarExistenciaEmpleado(IdEmpleado);
            string query = @"ESTADO_INCAPACIDAD";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IncapacidadesId = IncapacidadesId
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<IncapacidadesResponse>> Obtener()
        {
            string query = @"VER_INCAPACIDADES";
            var resultadoConsulta = await _sqlConnection.QueryAsync<IncapacidadesResponse>(query);
            return resultadoConsulta;
        }

        public async Task<IncapacidadesResponse> ObtenerPorId(Guid IncapacidadesId)
        {
            string query = @"VER_INCAPACIDADES_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<IncapacidadesResponse>(query,
                new { IncapacidadesId = IncapacidadesId });
            return resultadoConsulta.FirstOrDefault();
        }
    }
}
