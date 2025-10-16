using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IEmpleadoController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid IdEmpleado);

        Task<IActionResult> Agregar(EmpleadoPlanilla empleado);
        Task<IActionResult> Editar(Guid IdEmpleado, EmpleadoPlanilla empleado);
        Task<IActionResult> Eliminar(Guid IdEmpleado);
    }
}
