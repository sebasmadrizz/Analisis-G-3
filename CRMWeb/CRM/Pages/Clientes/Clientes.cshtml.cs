using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Clientes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Clientes
{
	public class ClientesModel : PageModel
	{
		private readonly IConfiguracion _configuracion;
		public IList<ClienteResponse> clientes { get; set; }

		[BindProperty]
		public Cliente cliente { get; set; }

		public ClientesModel(IConfiguracion configuracion)
		{
			_configuracion = configuracion;
		}

		public async Task OnGet()
		{
			string endpoint = _configuracion.ObtenerMetodo("EndPointsClientes", "ObtenerClientes");
			using var http = new HttpClient();
			var resp = await http.GetAsync(endpoint);
			if (!resp.IsSuccessStatusCode)
			{
				var body = await resp.Content.ReadAsStringAsync();
				clientes = new List<ClienteResponse>();
				return;
			}
			var json = await resp.Content.ReadAsStringAsync();
			clientes = JsonSerializer.Deserialize<List<ClienteResponse>>(json, new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			}) ?? new List<ClienteResponse>();
		}

		public async Task<IActionResult> OnPostEliminarCliente(Guid idCliente)
		{
			var endpoint = _configuracion.ObtenerMetodo("EndPointsClientes", "EliminarCliente");
			var url = string.Format(endpoint, idCliente);
			using var http = new HttpClient();
			var content = new StringContent("", Encoding.UTF8, "application/json");
			var resp = await http.PutAsync(url, content);
			var body = await resp.Content.ReadAsStringAsync();



			return RedirectToPage();
		}
		public async Task<IActionResult> OnPostAgregarCliente()
		{
			var endpoint = _configuracion.ObtenerMetodo("EndPointsClientes", "AgregarCliente");
			using var http = new HttpClient();

			var payload = new
			{
				TipoCliente = cliente.TipoCliente,
				Nombre = cliente.Nombre,
				Identificacion = cliente.Identificacion,
				Correo = cliente.Correo,
				Telefono = cliente.Telefono,
				Direccion = cliente.Direccion,
				FechaActualizacion = DateTime.UtcNow
			};

			var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
			Console.WriteLine(" JSON que se enviará al API:");
			Console.WriteLine(json);
			var resp = await http.PostAsJsonAsync(endpoint, payload);
			var body = await resp.Content.ReadAsStringAsync();

			return RedirectToPage();
		}


		public async Task<IActionResult> OnGetEditarFormulario(Guid idCliente)
		{
			var endpoint = _configuracion.ObtenerMetodo("EndPointsClientes", "ObtenerCliente");
			using var http = new HttpClient();

			var resp = await http.GetAsync(string.Format(endpoint, idCliente));
			if (!resp.IsSuccessStatusCode) return NotFound();

			var json = await resp.Content.ReadAsStringAsync();
			var modelo = JsonSerializer.Deserialize<Cliente>(json,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

			if (modelo is null) return NotFound();

			
			return Partial("_FormularioClienteEditar", modelo);
		}


		public async Task<IActionResult> OnPostEditarCliente(Cliente cliente)
		{

			if (!ModelState.IsValid)
				return Partial("_FormularioEditarCliente", cliente);

			string endpoint = _configuracion.ObtenerMetodo("EndPointsClientes", "EditarCliente");
			using var http = new HttpClient();
			

			var resp = await http.PutAsJsonAsync(string.Format(endpoint, cliente.ClienteId), new ClienteRequest
			{
				TipoCliente = cliente.TipoCliente,
				Nombre = cliente.Nombre,
				Identificacion = cliente.Identificacion,
				Correo = cliente.Correo,
				Telefono = cliente.Telefono,
				Direccion = cliente.Direccion,
				FechaActualizacion = DateTime.UtcNow,
			});

			if (resp.IsSuccessStatusCode)
				return new JsonResult(new { ok = true });

			
			
			return Partial("_FormularioEditarCliente", cliente);
		}
	}
}
