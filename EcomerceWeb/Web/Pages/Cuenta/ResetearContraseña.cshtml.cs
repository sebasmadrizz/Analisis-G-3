using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Web.Pages.Cuenta
{
    public class ResetearContraseñaModel : PageModel
    {
        private IConfiguracion _configuracion;
        [BindProperty]
        public ResetPassword resetPassword { get; set; } = default!;
        public bool accesoAprobado { get; set; }
        public bool peticionAprobada { get; set; }=true;
        public ResetearContraseñaModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public void OnGet(Guid userId, string token)
        {
            if (userId == Guid.Empty || string.IsNullOrEmpty(token))
            {
                accesoAprobado= false;
                return;
            }
            accesoAprobado = true;
            var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
            resetPassword = new ResetPassword
            {
                Token = tokenHash,
                
            };
            return;


        }
        public async Task<IActionResult> OnPost()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "ResetearContraseña");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Post, endpoint);
            var hash = Autenticacion.GenerarHash(resetPassword.Password);
            var hashString = Autenticacion.ObtenerHash(hash);
            resetPassword.Password= hashString;
            var hash2 = Autenticacion.GenerarHash(resetPassword.ConfirmPassword);
            var hashString2 = Autenticacion.ObtenerHash(hash2);
            resetPassword.ConfirmPassword= hashString2;
            var respuesta = await cliente.PostAsJsonAsync(endpoint, resetPassword);
            if (respuesta.StatusCode == HttpStatusCode.BadRequest)
            {
                peticionAprobada = false;
                return Page();



            }
            return RedirectToPage("../Cuenta/Login");
        }
    }
}
