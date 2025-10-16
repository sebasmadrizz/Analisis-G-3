using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IHorarioController
    {
        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerPorId(Guid HorarioId);
        Task<IActionResult> Agregar(Horario horario);

        Task<IActionResult> Editar(Guid HorarioId, Horario horario);

        Task<IActionResult> Eliminar(Guid HorarioId);
    }
}
