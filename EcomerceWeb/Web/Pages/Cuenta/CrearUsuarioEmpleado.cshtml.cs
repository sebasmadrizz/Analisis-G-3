using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Seguridad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Cuenta
{
    [Authorize(Roles ="1")]
    public class CrearUsuarioEmpleadoModel : PageModel
    {
        [BindProperty]
        public Usuario usuario { get; set; } = default!;
        private IConfiguracion _configuracion;
        public bool ExisteCorreo { get; set; }
        public CrearUsuarioEmpleadoModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var hash = Autenticacion.GenerarHash(usuario.Password);
            var hashString = Autenticacion.ObtenerHash(hash);
            usuario.PasswordHash = hashString;
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "CrearUsuarioEmpleado");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var respuesta = await cliente.PostAsJsonAsync<UsuarioBase>(endpoint, usuario);

            if (respuesta.StatusCode == HttpStatusCode.BadRequest)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var json = JsonSerializer.Deserialize<Dictionary<string, bool>>(resultado, opciones);
                ExisteCorreo = json["existeCorreo"];
                return Page();
            }

            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("../Cuenta/Login");
        }
    }
}
