using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IPuestosController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid PuestosId);
        Task<IActionResult> Agregar(Puestos puestos);

        Task<IActionResult> Editar(Guid PuestosId, Puestos puestos);

        Task<IActionResult> Eliminar(Guid PuestosId);

    }
}
