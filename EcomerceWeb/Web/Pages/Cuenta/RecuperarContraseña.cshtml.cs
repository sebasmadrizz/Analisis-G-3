using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Cuenta
{
    public class RecuperarContraseñaModel : PageModel
    {
        private IConfiguracion _configuracion;
        [BindProperty]
        public string Correo { get; set; }
        public RecuperarContraseñaModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task<IActionResult> OnPost()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "RecuperarContraseña");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Post, endpoint);
            var respuesta = await cliente.PostAsJsonAsync(endpoint, Correo);
            return Page();
        }
    }
}
