using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace Abstracciones.Interfaces.API
{
    public interface IAutenticacionController
    {
        Task<IActionResult> PostAsync([FromBody] Login login);
    }
}
