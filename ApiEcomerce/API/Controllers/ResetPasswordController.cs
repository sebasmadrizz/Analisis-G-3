using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Reglas;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResetPasswordController : ControllerBase, IResetPasswordController
    {
        private readonly IResetPasswordFlujo _resetPasswordFlujo;
        private readonly IUsuarioFlujo _usuarioFlujo;
        private readonly ILogger<ResetPasswordController> _logger;
        private readonly IGenerarResetTokenRegla _generarResetTokenRegla;
        private string _rutaWeb;
        private readonly IRepositorioResetPassword _repositorioResetPassword;
        private readonly ICorreoServicio _correoServicio;



        public ResetPasswordController(ICorreoServicio correoServicio,IRepositorioResetPassword repositorioResetPassword,IUsuarioFlujo usuarioFlujo,IResetPasswordFlujo resetPasswordFlujo, IGenerarResetTokenRegla generarResetTokenRegla, ILogger<ResetPasswordController> logger)
        {
            _resetPasswordFlujo = resetPasswordFlujo;
            _logger = logger;
            _generarResetTokenRegla = generarResetTokenRegla;
            _usuarioFlujo = usuarioFlujo;
            _rutaWeb= repositorioResetPassword.ObtenerRutaWeb();
            _correoServicio = correoServicio;
        }
        [AllowAnonymous]
        [HttpPost("RecuperarContrasena")]

        public async Task<IActionResult> RecuperarContraseña([FromBody]string correo)
        {
            Usuario user = await _usuarioFlujo.ObtenerUsuario(new Usuario { CorreoElectronico=correo});
            if (user == null)
            {
                return NotFound();
            }

            var myToken =  _generarResetTokenRegla.GenerarResetToken(correo);
            var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(myToken)));
            await _resetPasswordFlujo.GuardarResetPasswordToken(new ResetPasswordToken { UserId=user.Id, TokenHash= tokenHash, ExpiraEn= DateTime.UtcNow.AddHours(1) });
            var tokenLink = $"{_rutaWeb}/Cuenta/ResetearContraseña?userid={user.Id}&token={myToken}";
            await _correoServicio.EnviarCorreoRecuperacionAsync(user.NombreUsuario,correo, tokenLink);
            return Ok();



        }
        [AllowAnonymous]
        [HttpPost("ResetearContrasena")]
        public async Task<IActionResult> ResetearContraseña([FromBody] ResetPassword resetPassword)
        {
            if(resetPassword.Token==null)
                return BadRequest("Token es requerido");
            var resultado = await _resetPasswordFlujo.ResetPassword(resetPassword);//si el sp retorna un 0 es que hubo un error verificar esto
            if (resultado == 0)
                return BadRequest("No se pudo reestablecer la contraseña, el token es inválido o expiró.");
            return Ok(resultado);
        }
    }
}
