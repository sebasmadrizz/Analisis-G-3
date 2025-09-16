using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Carrito;
using Abstracciones.Modelos.Productos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Web.Pages.Productos
{
    public class DetalleProductoModel : PageModel
    {
        private IConfiguracion _configuracion;
        public Producto producto { get; set; } = default!;
        [BindProperty]
        public CarritoProducto carritoProducto { get; set; } = default!;

        public DetalleProductoModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task OnGet(Guid? IdProducto)
        {
            string endpoint = _configuracion.ObtenerMetodo("EndPointsProductos", "ObtenerProducto");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, IdProducto));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                producto = JsonSerializer.Deserialize<Producto>(resultado, opciones);
            }

        }
        public async Task<IActionResult> OnPost()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCarrito", "AgregarProductoCarrito");
            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var solicitud = new HttpRequestMessage(HttpMethod.Post, endpoint);
            
            var respuesta = await cliente.PostAsJsonAsync(endpoint, carritoProducto);
           
            if (respuesta.StatusCode == System.Net.HttpStatusCode.Created)
            {
                
                return RedirectToPage("../Carrito/Carrito");
            }
            else
            {
                
                TempData["ErrorStock"] = "No hay stock suficiente";


                return RedirectToPage(
                "./DetalleProducto",
                 new { IdProducto = carritoProducto.productosId }
                    );

            }





        }
    }
}
