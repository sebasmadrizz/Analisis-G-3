using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IEmpleadoController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid IdEmpleado);

        Task<IActionResult> Agregar(Empleado empleado);
        Task<IActionResult> Editar(Guid IdEmpleado, Empleado empleado);
        Task<IActionResult> Eliminar(Guid IdEmpleado);
    }
}
