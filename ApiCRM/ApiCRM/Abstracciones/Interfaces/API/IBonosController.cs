using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IBonosController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid BonosId);
        Task<IActionResult> Agregar(Bonos bonos);

        Task<IActionResult> Editar(Guid BonosId, Bonos bonos);

        Task<IActionResult> Eliminar(Guid BonosId);
    }
}
