using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Seguridad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Cuenta
{
    [Authorize]
    public class verCuentaModel : PageModel
    {
        [BindProperty]
        public Usuario usuario { get; set; } = default!;
        private IConfiguracion _configuracion;
        
        public verCuentaModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task OnGet()
        {
            string idUsuario = HttpContext.User.Claims.Where(c => c.Type == "idUsuario").FirstOrDefault().Value;
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "ObtenerUsuario");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, idUsuario));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                usuario = JsonSerializer.Deserialize<Usuario>(resultado, opciones);
                
            }
        }
        public async Task<IActionResult> OnPost()
        {
            string idUsuario = HttpContext.User.Claims.Where(c => c.Type == "idUsuario").FirstOrDefault().Value;
            
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "EditarUsuario");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var respuesta = await cliente.PutAsJsonAsync<Usuario>(string.Format(endpoint, idUsuario),usuario);
            respuesta.EnsureSuccessStatusCode();
            return Page();

        }

        [Authorize]
        public async Task<IActionResult> OnPostDesactivarAsync()
        {
            string idUsuarioToken = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "idUsuario")?.Value;

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "DesactivarUsuario");
            string url = string.Format(endpoint, idUsuarioToken);

            using var cliente = new HttpClient();
            var token = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Token")?.Value;
            cliente.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var respuesta = await cliente.PutAsync(url, null);
            string contenido = await respuesta.Content.ReadAsStringAsync();

            return new ContentResult
            {
                StatusCode = (int)respuesta.StatusCode,
                Content = contenido,
                ContentType = "application/json"
            };
        }


    }
}
