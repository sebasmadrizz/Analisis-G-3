using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Categoria;
using Abstracciones.Modelos.Productos;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Shared.Components
{
    public class MenuCategoriasVIewComponent:ViewComponent
    {
        private IConfiguracion _configuracion;
        public IList<CategoriaPadreConHijas> categorias { get; set; } = default!;
        public MenuCategoriasVIewComponent(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }
        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsCategorias", "ObtenerCategoriasPadreConHija");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();
            
                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                categorias = JsonSerializer.Deserialize<List<CategoriaPadreConHijas>>(resultado, opciones);
            return View(viewName, categorias);


        }














    }
}
