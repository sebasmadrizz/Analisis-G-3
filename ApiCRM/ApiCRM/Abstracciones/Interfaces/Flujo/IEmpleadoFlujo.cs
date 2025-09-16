using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IEmpleadoFlujo
    {
        Task<IEnumerable<EmpleadoResponse>> Obtener();
        Task<EmpleadoResponse> ObtenerPorId(Guid IdEmpleado);
        Task<Guid> Agregar(Empleado empleado);

        Task<Guid> Editar(Guid IdEmpleado, Empleado empleado);

        Task<Guid> Eliminar(Guid IdEmpleado);
    }
}
