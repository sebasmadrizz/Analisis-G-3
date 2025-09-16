using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Proveedores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Proveedor
{
    [Authorize(Roles = "1")]
    public class MostrarProveedoresModel : PageModel
	{
		private readonly IConfiguracion _configuracion;
		public IList<ProveedoresBase> proveedores { get; set; } = new List<ProveedoresBase>();
		public ProveedoresBase proveedor { get; set; } = new();

		public MostrarProveedoresModel(IConfiguracion configuracion)
		{
			_configuracion = configuracion;
		}

		public async Task OnGet()
{
    string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "ObtenerProveedores");

    using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);

            var resp = await http.GetAsync(endpoint);
    resp.EnsureSuccessStatusCode();

    if (resp.StatusCode == HttpStatusCode.OK)
    {
        var json = await resp.Content.ReadAsStringAsync();
        proveedores = JsonSerializer.Deserialize<List<ProveedoresBase>>(json, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        }) ?? new List<ProveedoresBase>();
    }
}

		
		public IActionResult OnGetFormularioModal()
		{
			var nuevoproveedor= new ProveedoresBase
			{
				PROVEEDOR_ID = Guid.NewGuid(),   
				ESTADO_ID = 1,                   
				Fecha_Registro = DateTime.UtcNow 
			};
			return Partial("_FormularioModalProveedor", nuevoproveedor);
		}


		public async Task<IActionResult> OnPostAgregarProveedor(ProveedoresBase proveedor)
		{
			if (!ModelState.IsValid)
				return Partial("_FormularioModalProveedor", proveedor);

			var endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "AgregarProveedor");
			using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);

            var content = new StringContent(JsonSerializer.Serialize(proveedor), Encoding.UTF8, "application/json");
			var resp = await http.PostAsync(endpoint, content);

			if (resp.IsSuccessStatusCode)
				return new JsonResult(new { ok = true });

			
			var body = await resp.Content.ReadAsStringAsync();
			ModelState.AddModelError(string.Empty, $"Error API: {body}");
			return Partial("_FormularioModalProveedor", proveedor);
		}


		public async Task<IActionResult> OnGetEditarFormulario(Guid PROVEEDOR_ID)
		{
			var endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "ObtenerProveedor");
			using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);

            var resp = await http.GetAsync(string.Format(endpoint, PROVEEDOR_ID));
			if (!resp.IsSuccessStatusCode) return NotFound();

			var json = await resp.Content.ReadAsStringAsync();
			var modelo = JsonSerializer.Deserialize<ProveedoresBase>(json,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			if (modelo is null) return NotFound();

			return Partial("_FormularioEditarProveedor", modelo);
		}

	
	
		public async Task<IActionResult> OnPostEditarProveedor(ProveedoresBase proveedor)
		{
			if (proveedor.PROVEEDOR_ID == Guid.Empty)
			{
				ModelState.AddModelError(string.Empty, "Identificador inválido.");
				return Partial("_FormularioEditarProveedor", proveedor);
			}

			if (!ModelState.IsValid)
				return Partial("_FormularioEditarProveedor", proveedor);

			string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "EditarProveedor");
			using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);

            var resp = await http.PutAsJsonAsync(string.Format(endpoint, proveedor.PROVEEDOR_ID), new ProveedoresRequest
			{
				PROVEEDOR_ID = proveedor.PROVEEDOR_ID,
				Nombre_PROVEEDOR = proveedor.Nombre_PROVEEDOR,
				Correo_ELECTRONICO = proveedor.Correo_ELECTRONICO,
				TIPO = proveedor.TIPO,
				Direccion = proveedor.Direccion,
				Telefono = proveedor.Telefono,
				ESTADO_ID = proveedor.ESTADO_ID,
				Fecha_Registro = proveedor.Fecha_Registro,
				Nombre_Contacto = proveedor.Nombre_Contacto
			});

			if (resp.IsSuccessStatusCode)
				return new JsonResult(new { ok = true });

			var body = await resp.Content.ReadAsStringAsync();
			ModelState.AddModelError(string.Empty, $"Error API: {body}");
			return Partial("_FormularioEditarProveedor", proveedor);
		}

		public async Task<IActionResult> OnPostEliminarProveedor(Guid idProveedor)
		{
			var endpoint = _configuracion.ObtenerMetodo("ApiEndPointsProveedores", "EliminarProveedor");
			var url = string.Format(endpoint, idProveedor);
			using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.User.Claims.Where(c => c.Type == "Token").FirstOrDefault().Value);
            var resp = await http.PutAsync(url, new StringContent("")); 

			resp.EnsureSuccessStatusCode();
			return RedirectToPage();
		}


	}
}