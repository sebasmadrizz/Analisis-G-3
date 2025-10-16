using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IHorasExtraController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid HorasExtrasId);
        Task<IActionResult> Agregar(HorasExtras horasExtras);

        Task<IActionResult> Editar(Guid HorasExtrasId, HorasExtras horasExtras);

        Task<IActionResult> Eliminar(Guid HorasExtrasId);
    }
}
