using System.Security.Claims;
using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using DA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.CarritoProducto;

namespace API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CarritoProductoController : ControllerBase, ICarritoProductoController
	{
		private readonly ICarritoProductoFlujo _carritoProductoFlujo;
		private readonly ILogger<CarritoProductoController> _logger;

		public CarritoProductoController(ICarritoProductoFlujo carritoProductoFlujo, ILogger<CarritoProductoController> logger)
		{
			_carritoProductoFlujo = carritoProductoFlujo;
			_logger = logger;
		}

		[Authorize]
		[HttpPost]
        public async Task<IActionResult> Agregar([FromBody] CarritoProductoRequest carritoProducto)
        {
            string idUsuarioStr = HttpContext.User.Claims
                          .FirstOrDefault(c => c.Type == "idUsuario")?.Value;

            if (string.IsNullOrEmpty(idUsuarioStr))
                return Unauthorized();

            if (!Guid.TryParse(idUsuarioStr, out Guid idUsuario))
                return BadRequest("IdUsuario inválido");

            try
            {
                var resultado = await _carritoProductoFlujo.Agregar(idUsuario, carritoProducto);
                return CreatedAtAction(nameof(ObtenerPorID), new { CarritoProductoId = resultado }, null);
            }
            catch (Exception ex) when (ex.Message.Contains("No hay stock suficiente"))
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

		[AllowAnonymous]
        [HttpPut("{CarritoProductoId}")]
        public async Task<IActionResult> Editar([FromRoute] Guid CarritoProductoId, [FromBody] CarritoProductoRequest carritoProducto)
        {
            try
            {
                var resultado = await _carritoProductoFlujo.Editar(CarritoProductoId, carritoProducto);
                return Ok(resultado);
            }
            catch (Exception ex) when (ex.Message.Contains("No hay stock suficiente"))
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }



        [AllowAnonymous]
        [HttpDelete("{CarritoProductoId}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid CarritoProductoId)
		{

			var resultado = await _carritoProductoFlujo.Eliminar(CarritoProductoId);
			return NoContent();
		}

        [AllowAnonymous]
        [HttpGet("{CarritoId}")]
		public async Task<IActionResult> ObtenerPorCarrito([FromRoute] Guid CarritoId)
		{
			var resultado = await _carritoProductoFlujo.ObtenerPorCarrito(CarritoId);
			return Ok(resultado);
		}
        [AllowAnonymous]
        [HttpGet("por-id/{CarritoProductoId}")]
		public async Task<IActionResult> ObtenerPorID([FromRoute] Guid CarritoProductoId)
		{
			var resultado = await _carritoProductoFlujo.ObtenerPorID(CarritoProductoId);

			if (resultado == null)
				return NotFound();

			return Ok(resultado);
		}
        [HttpPost("validarStock/{productoId}/{cantidadSolicitada}")]
		public async Task<IActionResult> ValidarStock( [FromRoute]Guid productoId, [FromRoute] int cantidadSolicitada)
		{
			try
			{
				var resultado = await _carritoProductoFlujo.ValidarStock(productoId, cantidadSolicitada);

				return Ok(resultado);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		
	}
}
