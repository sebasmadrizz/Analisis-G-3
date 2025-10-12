using Abstracciones.Modelos.Seguridad;
using Abstracciones.Modelos.Carrito;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using Abstracciones.Interfaces.Reglas;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pages.Carrito
{
	[Authorize]
	public class ProformaModel : PageModel
	{
		private readonly IConfiguracion _configuracion;

		[BindProperty]
		public Usuario UsuarioCliente { get; set; }

		// Propiedades para la cotización
		public string NombreCliente { get; set; }
		public string EmailCliente { get; set; }
		public string TelefonoCliente { get; set; }
		public string DireccionCliente { get; set; }
		public string AtencionCliente { get; set; }
		public string NumeroProforma { get; set; }

	
		public CarritoResponse Carrito { get; set; } = new CarritoResponse();
		public decimal SubTotal { get; set; }
		public decimal TotalIVA { get; set; }
		public decimal TotalGeneral { get; set; }

		
		public List<ProductoConIVA> ProductosConIVA { get; set; } = new List<ProductoConIVA>();

		public class ProductoConIVA
		{
			public CarritoProductoResponse Producto { get; set; }
			public decimal IVA { get; set; }
			public decimal SubtotalSinIVA { get; set; }
		}

		public ProformaModel(IConfiguracion configuracion)
		{
			_configuracion = configuracion;
		}

		public async Task OnGetAsync()
		{
			// Por el momento , hay que cmabiarlo para que sea secuencial
			NumeroProforma = $"PROF-{DateTime.Now:yyyyMMddHHmmss}";

			
			string idUsuario = HttpContext.User.Claims
				.Where(c => c.Type == "idUsuario")
				.FirstOrDefault()?.Value;

			if (!string.IsNullOrEmpty(idUsuario))
			{
				
				await ObtenerUsuarioDesdeAPI(idUsuario);

				
				await ObtenerCarritoDesdeAPI(idUsuario);

				
				CalcularTotales();

				if (UsuarioCliente != null)
				{
					NombreCliente = $"{UsuarioCliente.NombreUsuario} {UsuarioCliente.Apellido}";
					EmailCliente = UsuarioCliente.CorreoElectronico ?? "correo@ejemplo.com";
					TelefonoCliente = UsuarioCliente.Telefono ?? "No especificado";
					DireccionCliente = UsuarioCliente.Direccion ?? "Dirección no especificada";
					AtencionCliente = UsuarioCliente.NombreUsuario ?? "Cliente";
				}
				
			}
			
		}

		private void CalcularTotales()
		{
			ProductosConIVA = new List<ProductoConIVA>();
			SubTotal = 0;
			TotalIVA = 0;
			TotalGeneral = 0;

			if (Carrito?.Productos != null && Carrito.Productos.Any())
			{
				foreach (var producto in Carrito.Productos)
				{
					
					decimal subtotalLinea = producto.PrecioUnitario * producto.Cantidad;
					decimal ivaProducto = subtotalLinea * 0.13m; 
					ProductosConIVA.Add(new ProductoConIVA
					{
						Producto = producto,
						SubtotalSinIVA = subtotalLinea,
						IVA = ivaProducto
					});

					SubTotal += subtotalLinea;
					TotalIVA += ivaProducto;
				}

				TotalGeneral = SubTotal + TotalIVA;
			}
		}

		private async Task ObtenerUsuarioDesdeAPI(string idUsuario)
		{
			
				string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "ObtenerUsuario");
				var cliente = new HttpClient();

				var token = HttpContext.User.Claims
					.Where(c => c.Type == "Token")
					.FirstOrDefault()?.Value;

				cliente.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

				var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, idUsuario));
				var respuesta = await cliente.SendAsync(solicitud);

				if (respuesta.StatusCode == HttpStatusCode.OK)
				{
					var resultado = await respuesta.Content.ReadAsStringAsync();
					var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
					UsuarioCliente = JsonSerializer.Deserialize<Usuario>(resultado, opciones);
				}
			
			
		}

		private async Task ObtenerCarritoDesdeAPI(string idUsuario)
		{
			try
			{
				string endpointBase = _configuracion.ObtenerMetodo("ApiEndPointsCarrito", "ObtenerCarritoPorUsuario");
				string endpoint = $"{endpointBase}{idUsuario}";

				var cliente = new HttpClient();
				var token = HttpContext.User.Claims
					.Where(c => c.Type == "Token")
					.FirstOrDefault()?.Value;

				cliente.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

				var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);
				var respuesta = await cliente.SendAsync(solicitud);

				if (respuesta.IsSuccessStatusCode)
				{
					var resultado = await respuesta.Content.ReadAsStringAsync();
					var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
					Carrito = JsonSerializer.Deserialize<CarritoResponse>(resultado, opciones) ?? new CarritoResponse();
				}
			}
			catch (Exception)
			{
				Carrito = new CarritoResponse { Productos = new List<CarritoProductoResponse>(), Total = 0 };
			}
		}

		
	}
}