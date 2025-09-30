using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IResetPasswordController
    {
        Task<IActionResult> RecuperarContraseña(string correo);
        Task<IActionResult> ResetearContraseña(ResetPassword resetPassword);
    }
}
