using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Abstracciones.Modelos.Categoria;
using Abstracciones.Modelos.Productos;
using Abstracciones.Modelos.Proveedores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    [Authorize(Roles = "1")]
    public class InventarioModel : PageModel
    {
        private IConfiguracion _configuracion;
        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _environment;

        [BindProperty]
        public ProductosRequest productoRequest { get; set; }
        [BindProperty]
        public IFormFile imagen { get; set; } = null!;
        public IList<Producto> productos { get; set; } = default!;
        [BindProperty]
        public List<SelectListItem> proveedores { get; set; } = default!;
        public List<SelectListItem> categoriasSelect { get; set; } = default!;
        public ProductoConImagenRequest objetoEnviar { get; set; }


        [BindProperty]
        public ProductoPaginado ProductosPaginados { get; set; } = default!;
        [BindProperty(SupportsGet = false)]
        public string opcionExportar{ get; set; }
        [BindProperty(SupportsGet = false)]
        public Guid categoriaID{ get; set; }
        [BindProperty]
        public string SearchTerm { get; set; }
        [BindProperty]
        public ProductosBuscados productosBuscados { get; set; } = default!;


        public InventarioModel(IConfiguracion configuracion, Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            _configuracion = configuracion;
            _environment = environment;
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
                await ObtenerProveedoresAsync();
                await ObtenerCategoriasAsync();


            }
        }
        public async Task OnPostBuscarProductos(int PagesIndex = 1, int PageSize = 10)
        {
            if (SearchTerm != "")
            {
                
                string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "BusquedasIndex");
                string baseUrl = string.Format(endpoint, PagesIndex, PageSize);

                var finalUrl = QueryHelpers.AddQueryString(baseUrl, "searchTerm", SearchTerm ?? string.Empty);

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
        public async Task<IActionResult> OnPostCrearProducto()
        {
            objetoEnviar= await ActualizarObjetoAEnviar();


            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "AgregarProducto");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var solicitud = new HttpRequestMessage(HttpMethod.Post, endpoint);
            var respuesta = await cliente.PostAsJsonAsync(endpoint, objetoEnviar);

            if (!respuesta.IsSuccessStatusCode)
            {
                TempData["CrearProductoExito"] = false;
                return RedirectToPage();
            }

            TempData["CrearProductoExito"] = true;
            return RedirectToPage();
        }
        public async Task ObtenerProveedoresAsync()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "ObtenerProveedores");
            var cliente = new HttpClient();
           
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var resultadoDeserealizado = JsonSerializer.Deserialize<List<ProveedoresResponse>>(resultado, opciones);
                proveedores = resultadoDeserealizado.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.PROVEEDOR_ID.ToString(),
                                      Text = a.Nombre_PROVEEDOR.ToString()
                                  }).ToList();


            }
        }

        public async Task ObtenerCategoriasAsync()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "ObtenerCategoriasTotales");
            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var resultadoDeserealizado = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones);
                categoriasSelect = resultadoDeserealizado.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.categoriasId.ToString(),
                                      Text = a.nombre.ToString()
                                  }).ToList();


            }
        }

        public async Task<IActionResult> OnGetFormularioModalEditar(Guid? idProducto)
        {
            if (idProducto == Guid.Empty)
                return NotFound();
            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ObtenerProducto");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, idProducto));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                productoRequest = JsonSerializer.Deserialize<ProductosRequest>(resultado, opciones);
                productoRequest.IdProducto = idProducto;
                

                return Partial("_FormularioModalEditar", productoRequest);
            }
            else
            {
                return NotFound();
            }

        }
        public async Task<ActionResult> OnPostEditarProducto()
        {
            objetoEnviar = await ActualizarObjetoAEnviar();

            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "EditarProducto");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var respuesta = await cliente.PutAsJsonAsync<ProductoConImagenRequest>(string.Format(endpoint, productoRequest.IdProducto), objetoEnviar);
            respuesta.EnsureSuccessStatusCode();
            return new JsonResult(new { success = true });


        }

        private string ObtenerTipo(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
        private async Task<ProductoConImagenRequest> ActualizarObjetoAEnviar()
        {

            if (imagen != null)
            {
                var file = Path.Combine(_environment.ContentRootPath, "Imagenes", imagen.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await imagen.CopyToAsync(fileStream);
                }

                byte[] contenido = System.IO.File.ReadAllBytes(file);
                Documento documento = new Documento() { Id = Guid.NewGuid(), Nombre = imagen.FileName, Contenido = contenido, Tipo = ObtenerTipo(imagen.FileName) };
                return  new ProductoConImagenRequest()
                {
                    Productos = productoRequest,
                    Imagen = documento
                };

            }
            else
            {

                return new ProductoConImagenRequest()
                {
                    Productos = productoRequest,
                    Imagen = null
                };
            }

        }
        public async Task<ActionResult> OnPostEliminarProducto(Guid idProducto)
        {

            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "EliminarProducto");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var solicitud = new HttpRequestMessage(HttpMethod.Put, string.Format(endpoint, idProducto));
            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage();


        }
        public async Task<IActionResult> ExportExcelIventarioCompleto()
        {
            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ExportarExcel");
            using var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);

            var respuesta = await cliente.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
            respuesta.EnsureSuccessStatusCode();

            var stream = await respuesta.Content.ReadAsStreamAsync();

            
            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Inventario.xlsx"
            );
        }
        public async Task<IActionResult> ExportPDFIventarioCompleto()
        {
            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ExportarPDF");
            using var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);

            var respuesta = await cliente.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
            respuesta.EnsureSuccessStatusCode();

            var stream = await respuesta.Content.ReadAsStreamAsync();


            return File(
                stream,
                "application/pdf",
                "Inventario.pdf"
                        );
        }
        public async Task<IActionResult> ExportExcelIventarioXCategoria()
        {
            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ExportarExcelXCategoria");
            endpoint = string.Format(endpoint, categoriaID);
            using var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);

            var respuesta = await cliente.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
            respuesta.EnsureSuccessStatusCode();

            var stream = await respuesta.Content.ReadAsStreamAsync();


            return File(
                stream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Inventario.xlsx"
            );
        }
        public async Task<IActionResult> ExportPDFIventarioXCategoria()
        {
            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ExportarPDFXCategoria");
            using var cliente = new HttpClient();
            endpoint = string.Format(endpoint, categoriaID);
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);

            var respuesta = await cliente.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
            respuesta.EnsureSuccessStatusCode();

            var stream = await respuesta.Content.ReadAsStreamAsync();


            return File(
                stream,
                "application/pdf",
                "Inventario.pdf"
                        );
        }
        public async Task<IActionResult> OnPostExportExcel()
        {
            if (opcionExportar== "todo")
            {
                return await ExportExcelIventarioCompleto();
            }
            else
            {
                return await ExportExcelIventarioXCategoria();

            }

        }
        public async Task<IActionResult> OnPostExportPDF()
        {
            if (opcionExportar == "todo")
            {
                return await ExportPDFIventarioCompleto();
            }
            else
            {
                return await ExportPDFIventarioXCategoria();

            }
        }

    }
}
