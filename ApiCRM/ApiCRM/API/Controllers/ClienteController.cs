using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using DA;
using Flujo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{

	[Route("api/[controller]")]
	[ApiController]
	public class ClienteController : ControllerBase, IClienteController
	{
		
		
			private readonly IClienteFlujo _clienteFlujo;
			private readonly ILogger<ClienteController> _logger;
			public ClienteController(IClienteFlujo clienteFlujo, ILogger<ClienteController> logger)
			{
				_clienteFlujo = clienteFlujo;
				_logger = logger;
			}
		
			[HttpPost]
			public async Task<IActionResult> Agregar([FromBody] Cliente cliente)
			{
				var resultado = await _clienteFlujo.Agregar(cliente);
				return CreatedAtAction(nameof(ObtenerPorId), new { CLIENTE_ID = resultado }, null);
			}


			[HttpGet]
			public async Task<IActionResult> Obtener()
			{
				var resultado = await _clienteFlujo.Obtener();
				if (!resultado.Any())
					return NoContent();
				return Ok(resultado);
			}


		
			[HttpGet("{CLIENTE_ID}")]

			public async Task<IActionResult> ObtenerPorId([FromRoute] Guid CLIENTE_ID)
			{
				var resultado = await _clienteFlujo.ObtenerPorId(CLIENTE_ID);
				return Ok(resultado);
			}
			
			[HttpPut("{CLIENTE_ID}")]
			public async Task<IActionResult> Editar([FromRoute] Guid CLIENTE_ID, [FromBody] Cliente cliente)
			{
				if (!await VerificarExistenciaCliente(CLIENTE_ID))
					return NotFound("El cliente no esta registrado");
				var resultado = await _clienteFlujo.Editar(CLIENTE_ID, cliente);
				return Ok(resultado);
			}

		
			[HttpPut("desactivar-cliente/{CLIENTE_ID}")]
			public async Task<IActionResult> Eliminar([FromRoute] Guid CLIENTE_ID)
			{
				if (!await VerificarExistenciaCliente(CLIENTE_ID))
					return NotFound("El cliente no esta registrado");
				var resultado = await _clienteFlujo.Eliminar(CLIENTE_ID);
				return Ok(resultado);
			}



			private async Task<bool> VerificarExistenciaCliente(Guid Id)
			{
				var ResultadoValidacion = false;
				var resultadoVehiculoExiste = await _clienteFlujo.ObtenerPorId(Id);
				if (resultadoVehiculoExiste != null)
					ResultadoValidacion = true;
				return ResultadoValidacion;
			}

	
	
	}
}
