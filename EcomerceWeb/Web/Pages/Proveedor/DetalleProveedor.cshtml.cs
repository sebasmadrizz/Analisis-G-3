using System.Net;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Proveedores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Proveedor
{
	public class DetalleProveedorModel : PageModel
	{
		private IConfiguracion _configuracion;
		public ProveedoresBase proveedor { get; set; } = default!;

		public DetalleProveedorModel(IConfiguracion configuracion)
		{
			_configuracion = configuracion;
		}

		public async Task OnGet(Guid? PROVEEDOR_ID)
		{
			

			var endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "ObtenerProveedor")
										.Replace("{0}", PROVEEDOR_ID.ToString());
			var cliente = new HttpClient();
			var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, PROVEEDOR_ID));
			var respuesta = await cliente.SendAsync(solicitud);
			respuesta.EnsureSuccessStatusCode();
			if (respuesta.StatusCode == HttpStatusCode.OK)
			{
				var resultado = await respuesta.Content.ReadAsStringAsync();
				var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
				proveedor = JsonSerializer.Deserialize<ProveedoresBase>(resultado, opciones);
			}

		}
	}
}



