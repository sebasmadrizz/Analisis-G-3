using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Proveedores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Proveedor
{
    public class EliminarProveedorModel : PageModel
    {
		private IConfiguracion _configuracion;
		public ProveedoresBase proveedor { get; set; } = default!;

		public EliminarProveedorModel(IConfiguracion configuracion)
		{
			_configuracion = configuracion;
		}

		public async Task<IActionResult> OnGetAsync(Guid? PROVEEDOR_ID)
		{
			if (PROVEEDOR_ID == null || PROVEEDOR_ID == Guid.Empty)
				return NotFound();

			string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "ObtenerProveedor").Replace("{0}", PROVEEDOR_ID.ToString());

			using var cliente = new HttpClient();
			var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, PROVEEDOR_ID));

			var respuesta = await cliente.SendAsync(solicitud);
			respuesta.EnsureSuccessStatusCode();

			
			var resultado = await respuesta.Content.ReadAsStringAsync();
			var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			


			
			proveedor = JsonSerializer.Deserialize<ProveedoresBase>(resultado, opciones);
			return Page();
		}

		public async Task<ActionResult> OnPost(Guid? PROVEEDOR_ID)
		{

			if (PROVEEDOR_ID == Guid.Empty)
				NotFound();
			if (!ModelState.IsValid)
				Page();
			string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores",
						"EliminarProveedor");
			var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var solicitud = new HttpRequestMessage(HttpMethod.Delete, string.Format(endpoint, PROVEEDOR_ID));

			var respuesta = await cliente.SendAsync(solicitud);
			respuesta.EnsureSuccessStatusCode();
			return RedirectToPage("./MostrarProveedores");
		}
	}
}

