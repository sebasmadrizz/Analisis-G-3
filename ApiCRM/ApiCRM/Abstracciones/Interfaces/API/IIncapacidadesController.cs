using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IIncapacidadesController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid IncapacidadesId);
        Task<IActionResult> Agregar(Incapacidades incapacidad);

        Task<IActionResult> Editar(Guid IncapacidadesId, Incapacidades incapacidad);

        Task<IActionResult> Eliminar(Guid IncapacidadesId);
    }
}
