using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IEmpleadoDA
    {
        Task<IEnumerable<EmpleadoResponse>> Obtener();
        Task<EmpleadoResponse> ObtenerPorId(Guid IdEmpleado);
        Task<Guid> Agregar(Empleado empleado);

        Task<Guid> Editar(Guid IdEmpleado, Empleado empleado);

        Task<Guid> Eliminar(Guid IdEmpleado);
    }
}
