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

        public async Task<Guid> Agregar(EmpleadoPlanilla empleado)
        {
            string query = @"AGREGAR_EMPLEADOPLANILLA";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                EmpleadoId = Guid.NewGuid(),
                Cedula=empleado.Cedula,
                Nombre = empleado.Nombre,
                Apellido= empleado.Apellido,
                CorreoElectronico = empleado.CorreoElectronico,
                Telefono = empleado.Telefono,
                FechaIngreso = empleado.FechaIngreso,
                Padecimientos = empleado.Padecimientos,
                PuestoId = empleado.PuestoId,
                CuentaBancaria = empleado.CuentaBancaria,
                HorarioId = empleado.HorarioId,
                Sueldo = empleado.Sueldo,
                Banco= empleado.banco,
                tipoCuenta= empleado.tipoCuenta,
                EstadoId = 1,
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid IdEmpleado, EmpleadoPlanilla empleado)
        {
            //await VerificarExistenciaEmpleado(IdEmpleado);


            string query = @"EDITAR_EMPLEADOPLANILLA";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                EmpleadoId = IdEmpleado,
                Cedula = empleado.Cedula,
                Nombre = empleado.Nombre,
                Apellido = empleado.Apellido,
                CorreoElectronico = empleado.CorreoElectronico,
                Telefono = empleado.Telefono,
                FechaIngreso = empleado.FechaIngreso,
                Padecimientos = empleado.Padecimientos,
                PuestoId = empleado.PuestoId,
                CuentaBancaria = empleado.CuentaBancaria,
                HorarioId = empleado.HorarioId,
                Sueldo = empleado.Sueldo,
                Banco = empleado.banco,
                tipoCuenta = empleado.tipoCuenta,


                
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
