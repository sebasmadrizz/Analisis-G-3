using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class EmpleadoFlujo: IEmpleadoFlujo
    {
        private readonly IEmpleadoDA _empleadoDA;
        public EmpleadoFlujo(IEmpleadoDA empleadoDA)
        {
            _empleadoDA = empleadoDA;
        }

        public async Task<Guid> Agregar(Empleado empleado)
        {
            return await _empleadoDA.Agregar(empleado);
        }

        public async Task<Guid> Editar(Guid IdEmpleado, Empleado empleado)
        {
            return await _empleadoDA.Editar(IdEmpleado, empleado);
        }

        public async Task<Guid> Eliminar(Guid IdEmpleado)
        {
            return await _empleadoDA.Eliminar(IdEmpleado);
        }

        public async Task<IEnumerable<EmpleadoResponse>> Obtener()
        {
            return await _empleadoDA.Obtener();
        }

        public async Task<EmpleadoResponse> ObtenerPorId(Guid IdEmpleado)
        {
            return await _empleadoDA.ObtenerPorId(IdEmpleado);
        }
    }
}
