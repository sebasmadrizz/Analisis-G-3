using System.Net;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Proveedores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages.Proveedor
{
	public class EditarProveedorModel : PageModel
	{
		[BindProperty]
		public ProveedoresBase proveedor { get; set; } = new();

		public ProveedoresRequest proveedorRequest { get; set; } = default!;
		private readonly IConfiguracion _configuracion;
		public EditarProveedorModel(IConfiguracion configuracion)
		{
			_configuracion = configuracion;
		}


		public async Task<ActionResult> OnGet(Guid? PROVEEDOR_ID)
		{
			if (PROVEEDOR_ID == null)
				return NotFound();

			var endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "ObtenerProveedor");
										
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
			return Page();
		}

		public async Task<ActionResult> OnPost()
		{
			if (proveedor.PROVEEDOR_ID == Guid.Empty)
				return NotFound();

			if (!ModelState.IsValid)
				return Page();

			string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "EditarProveedor");
			var cliente = new HttpClient();

			var respuesta = await cliente.PutAsJsonAsync<ProveedoresRequest>(string.Format(endpoint, proveedor.PROVEEDOR_ID.ToString()),
			new ProveedoresRequest
			{
				PROVEEDOR_ID = proveedor.PROVEEDOR_ID,
				Nombre_PROVEEDOR = proveedor.Nombre_PROVEEDOR,
				Correo_ELECTRONICO = proveedor.Correo_ELECTRONICO,
				TIPO = proveedor.TIPO,
				Direccion = proveedor.Direccion,
				Telefono = proveedor.Telefono,
				ESTADO_ID = proveedor.ESTADO_ID,
				Fecha_Registro = proveedor.Fecha_Registro,
				Nombre_Contacto = proveedor.Nombre_Contacto,


			});
			respuesta.EnsureSuccessStatusCode();
			return RedirectToPage("./MostrarProveedores");
		}
	}
}
