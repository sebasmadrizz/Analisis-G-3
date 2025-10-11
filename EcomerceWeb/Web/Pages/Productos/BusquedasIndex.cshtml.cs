using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Categoria;
using Abstracciones.Modelos.Productos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class BusquedasIndexModel : PageModel
    {
        private IConfiguracion _configuracion;
        public IList<Producto> productos { get; set; } = default!;
        

        [BindProperty]
        public ProductoPaginado ProductosPaginados { get; set; } = default!;
        [BindProperty]
        public ProductosBuscados productosBuscados { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }


        public BusquedasIndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task OnGet( int PagesIndex = 1, int PageSize = 10,string searchTerm="")
        {
            if (searchTerm!="")
            {
                SearchTerm = searchTerm;
                string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "BusquedasIndex");
                string baseUrl = string.Format(endpoint, PagesIndex, PageSize);

                var finalUrl = QueryHelpers.AddQueryString(baseUrl, "searchTerm", searchTerm ?? string.Empty);

                var cliente = new HttpClient();
                var solicitud = new HttpRequestMessage(HttpMethod.Get, finalUrl);

                var respuesta = await cliente.SendAsync(solicitud);
                respuesta.EnsureSuccessStatusCode();
                if (respuesta.StatusCode == HttpStatusCode.OK)
                {
                    var resultado = await respuesta.Content.ReadAsStringAsync();
                    var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    productosBuscados = JsonSerializer.Deserialize<ProductosBuscados>(resultado, opciones);
                    ProductosPaginados = productosBuscados.data;
                    ProductosPaginados.PageSize = PageSize;
                    productos = ProductosPaginados.Items;

                }

            }
        }
        

        
    }
}
