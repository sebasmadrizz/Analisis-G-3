using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Categoria;
using Abstracciones.Modelos.Productos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Web.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private IConfiguracion _configuracion;
        public IList<Producto> productos { get; set; } = default!;
        public IList<Categoria> categorias { get; set; } = new List<Categoria>();

        [BindProperty]
        public ProductoPaginado ProductosPaginados { get; set; } = default!;
       
        

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task OnGet(int PagesIndex = 1, int PageSize = 5)
        {


            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ObtenerProductosPaginados");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, PagesIndex, PageSize));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ProductosPaginados = JsonSerializer.Deserialize<ProductoPaginado>(resultado, opciones);

                productos = ProductosPaginados.Items;
                ProductosPaginados.PageSize = PageSize;

            }
        }
        public async Task OnPost(int PagesIndex = 1, int PageSize = 5)
        {


            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ObtenerProductosPaginados");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, PagesIndex, PageSize));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ProductosPaginados = JsonSerializer.Deserialize<ProductoPaginado>(resultado, opciones);

                productos = ProductosPaginados.Items;

            }
        }

        public async Task<IActionResult> OnGetObtenerCategoriasPadres()
        {
            var cliente = new HttpClient();
            string endpointTodas = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "VerPadres");
            var respuestaTodas = await cliente.GetAsync(endpointTodas);
            respuestaTodas.EnsureSuccessStatusCode();
            if (respuestaTodas.StatusCode == HttpStatusCode.NoContent)
            {

                return new JsonResult(new { padres = false });
            }
            var resultadoTodas = await respuestaTodas.Content.ReadAsStringAsync();
            categorias = JsonSerializer.Deserialize<List<Categoria>>(resultadoTodas,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            return new JsonResult(categorias);
        }
        public async Task<IActionResult> OnGetObtenerCategoriasHijas(Guid id)
        {
            

            var cliente = new HttpClient();
            string endpointHijas = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "ObtenerHijas");

            var url = string.Format(endpointHijas, id);

            var respuesta = await cliente.GetAsync(url);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {

                return new JsonResult(new { tieneHijas = false });
            }
            var resultado = await respuesta.Content.ReadAsStringAsync();
            var hijas = JsonSerializer.Deserialize<List<Categoria>>(resultado,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Categoria>();

            return new JsonResult(new { tieneHijas = true, categorias = hijas });
        }



    }
}
