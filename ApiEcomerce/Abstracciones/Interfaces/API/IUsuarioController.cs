using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace Abstracciones.Interfaces.API
{
    public interface IUsuarioController
    {
        Task<IActionResult> PostAsync([FromBody] Modelos.Usuario usuario);
        Task<IActionResult> CrearUsuarioEmpleado([FromBody] Modelos.Usuario usuario);
        Task<IActionResult> ObtenerUsuario([FromBody] Modelos.Usuario usuario);
        Task<IActionResult> EditarUsuario( Guid idUsuario, [FromBody] Modelos.UsuarioEditar usuario);
        Task<IActionResult> DetalleUsuario(Guid idUsuario);

        Task<IActionResult> Desactivar(Guid idUsuario);
    }
}
