using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Categoria;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Web.Pages.Categorias
{
    [Authorize(Roles = "1")]
    public class CategoriasModel : PageModel
    {
        
        private IConfiguracion _configuracion;
        public IList<Categoria> categorias { get; set; } = new List<Categoria>();

        [BindProperty]
        public VerificarCategoriaResponse verificar { get; set; } = default!;
        public bool SinCategorias { get; set; } = false;

        [BindProperty]
        public Categoria Categoria { get; set; } = default!;
        public IList<Categoria> categoriasPadres { get; set; } = new List<Categoria>();
        public CategoriasModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task<IActionResult> OnGet()
        {
            var cliente = new HttpClient();
            string endpointTodas = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "ObtenerCategoriasTotales");
            var respuestaTodas = await cliente.GetAsync(endpointTodas);
            respuestaTodas.EnsureSuccessStatusCode();
            if (respuestaTodas.StatusCode == HttpStatusCode.NoContent)
            {

                SinCategorias = true;
                return Page();
            }
            var resultadoTodas = await respuestaTodas.Content.ReadAsStringAsync();
            categorias = JsonSerializer.Deserialize<List<Categoria>>(resultadoTodas,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            string endpointPadres = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "VerPadres");
            var respuestaPadres = await cliente.GetAsync(endpointPadres);
            respuestaPadres.EnsureSuccessStatusCode();
            var resultadoPadres = await respuestaPadres.Content.ReadAsStringAsync();
            categoriasPadres = JsonSerializer.Deserialize<List<Categoria>>(resultadoPadres,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            return Page();
        }


        public async Task<ActionResult> OnPostAgregarCategoria()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "AgregarCategoria");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var respuesta = await cliente.PostAsJsonAsync(endpoint, Categoria);
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Categorias");
        }

        public async Task<ActionResult> OnPostEliminar(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "DesactivarCategorias");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var solicitud = new HttpRequestMessage(HttpMethod.Put, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Categorias");
        }

        public async Task<IActionResult> OnGetVerificar(Guid id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "ContarHijas");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var verificar = JsonSerializer.Deserialize<VerificarCategoriaResponse>(resultado, opciones);

            return new JsonResult(verificar);
        }

        public async Task<IActionResult> OnGetContarHijasTotales(Guid id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "HijasTotales");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var verificar = JsonSerializer.Deserialize<VerificarCategoriaResponse>(resultado, opciones);

            return new JsonResult(verificar);
        }

        public async Task<ActionResult> OnPostActivarPadreHijas(Guid? id, bool activarHijas)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "ActivarPadreHijas");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var url = string.Format(endpoint, id, activarHijas.ToString().ToLower());

            var solicitud = new HttpRequestMessage(HttpMethod.Put, url);
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Categorias");
        }
        public async Task<ActionResult> OnPostActivarHijas(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "ActivarHijas");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var url = string.Format(endpoint, id);

            var solicitud = new HttpRequestMessage(HttpMethod.Put, url);
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Categorias");
        }

        public async Task<IActionResult> OnGetObtenerPorId(Guid? id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "ObtenerCategoria");
            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Categoria = JsonSerializer.Deserialize<Categoria>(resultado, opciones);

                return new JsonResult(Categoria);
            }
            return NotFound();
        }
        public async Task<IActionResult> OnPostEditarCategoria(Guid id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "EditarCategoria");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var url = string.Format(endpoint, id);

            var respuesta = await cliente.PutAsJsonAsync(url, Categoria);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Categorias");
        }
    }
}

