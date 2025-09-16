using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class EmpleadoDA : IEmpleadoDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;
        public EmpleadoDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid> Agregar(Empleado empleado)
        {
            string query = @"AGREGAR_EMPLEADO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdEmpleado = Guid.NewGuid(),
                Cedula=empleado.Cedula,
                Nombre = empleado.NombreCompleto,
                CorreoElectronico = empleado.CorreoElectronico,
                Puesto = empleado.Puesto,
                Padecimientos = empleado.Padecimientos,
                CuentaBancaria = empleado.CuentaBancaria,
                TipoContrato = empleado.TipoContrato,
                Jornada = empleado.Jornada,
                Telefono = empleado.Telefono,
                EstadoId = 1,
                FechaRegistro = DateTime.Now,
                FechaIngreso=empleado.FechaIngreso


            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid IdEmpleado, Empleado empleado)
        {
            await VerificarExistenciaEmpleado(IdEmpleado);


            string query = @"EDITAR_EMPLEADO";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdEmpleado = IdEmpleado,
                Nombre = empleado.NombreCompleto,
                CorreoElectronico = empleado.CorreoElectronico,
                Puesto = empleado.Puesto,
                Padecimientos = empleado.Padecimientos,
                CuentaBancaria = empleado.CuentaBancaria,
                TipoContrato = empleado.TipoContrato,
                Jornada = empleado.Jornada,
                Telefono = empleado.Telefono
            });

            return resultado;
        }

        public async Task<Guid> Eliminar(Guid IdEmpleado)
        {
            await VerificarExistenciaEmpleado(IdEmpleado);
            string query = @"ESTADO_EMPLEADO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdEmpleado = IdEmpleado
            });
            return resultadoConsulta;
        }

        public async Task<IEnumerable<EmpleadoResponse>> Obtener()
        {
            string query = @"VER_EMPLEADOS";
            var resultadoConsulta = await _sqlConnection.QueryAsync<EmpleadoResponse>(query);
            return resultadoConsulta;
        }

        public async Task<EmpleadoResponse> ObtenerPorId(Guid IdEmpleado)
        {
            string query = @"VER_EMPLEADO_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<EmpleadoResponse>(query,
                new { IdEmpleado = IdEmpleado });
            return resultadoConsulta.FirstOrDefault();
        }
        private async Task VerificarExistenciaEmpleado(Guid IdEmpleado)
        {
            EmpleadoResponse? resutadoConsulta = await ObtenerPorId(IdEmpleado);
            if (resutadoConsulta == null)
                throw new Exception("no se encontro el empleado");
        }
    }
}
