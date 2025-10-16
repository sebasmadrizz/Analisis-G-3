using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IEmpleadoDA
    {
        Task<IEnumerable<EmpleadoResponse>> Obtener();
        Task<EmpleadoResponse> ObtenerPorId(Guid IdEmpleado);
        Task<Guid> Agregar(EmpleadoPlanilla empleado);

        Task<Guid> Editar(Guid IdEmpleado, EmpleadoPlanilla empleado);

        Task<Guid> Eliminar(Guid IdEmpleado);
    }
}
