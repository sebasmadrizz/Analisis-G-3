using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Categoria;
using Abstracciones.Modelos.Productos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class ProductosXCategoriaModel : PageModel
    {
        private IConfiguracion _configuracion;
        public IList<Producto> productos { get; set; } = default!;
        public IList<Categoria> categorias { get; set; } = new List<Categoria>();

        [BindProperty]
        public ProductoPaginado ProductosPaginados { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public Guid CategoriaId { get; set; }



        public ProductosXCategoriaModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task OnGet(Guid categoriaId, int PagesIndex = 1, int PageSize = 5)
        {
            CategoriaId = categoriaId;


            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ObtenerProductosXCategoria");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, categoriaId, PagesIndex, PageSize));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                ProductosPaginados = JsonSerializer.Deserialize<ProductoPaginado>(resultado, opciones);
                ProductosPaginados.PageSize = PageSize;
                productos = ProductosPaginados.Items;

            }
        }
        

        
    }
}
