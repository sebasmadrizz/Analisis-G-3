using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IAusenciasController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid AusenciasId);
        Task<IActionResult> Agregar(Ausencias ausencias);

        Task<IActionResult> Editar(Guid AusenciasId, Ausencias ausencias);

        Task<IActionResult> Eliminar(Guid AusenciasId);
    }
}
