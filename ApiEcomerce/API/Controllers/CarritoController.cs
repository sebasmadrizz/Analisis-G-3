using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using DA;
using Flujo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Carrito;


namespace API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class CarritoController : ControllerBase, ICarritoController
	{
		private readonly ICarritoFlujo _carritoFlujo;
		private readonly ILogger<CarritoController> _logger;
        private readonly ICorreoServicio _correoServicio;


        public CarritoController(ICarritoFlujo carritoFlujo, ILogger<CarritoController> logger, ICorreoServicio correoServicio)
		{
			_carritoFlujo = carritoFlujo;
			_logger = logger;
            _correoServicio = correoServicio;
        }

        [HttpPost]
		public async Task<IActionResult> Agregar([FromBody] CarritoBase carrito)
		{
			var resultado = await _carritoFlujo.Agregar(carrito);
			return CreatedAtAction(nameof(ObtenerPorID), new { CarritoId = resultado }, null);

		}



        [HttpPut("{CarritoId}")]
		public async Task<IActionResult> Editar([FromRoute] Guid CarritoId, [FromBody] CarritoBase carrito)
		{
            if (!await VerificarExistenciaCarrito(CarritoId))
                return NotFound("el carrito no existe");
            var resultado = await _carritoFlujo.Editar(CarritoId, carrito);
			return Ok(resultado);
		}



        [HttpDelete("{CarritoId}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid CarritoId)
		{
            if (!await VerificarExistenciaCarrito(CarritoId))
                return NotFound("el carrito no existe");
            var resultado = await _carritoFlujo.Eliminar(CarritoId);
			return NoContent();
		}

        [Authorize]
        [HttpGet("por-user/{UsuarioId}")]
		public async Task<IActionResult> ObtenerPorUsuario([FromRoute] Guid UsuarioId)
		{
			var resultado = await _carritoFlujo.ObtenerPorUsuario(UsuarioId);
            if (resultado == null)
                return NotFound();
            return Ok(resultado);
		}

        [HttpGet("por-id/{CarritoId}")]
		public async Task<IActionResult> ObtenerPorID([FromRoute] Guid CarritoId)
		{
			var resultado = await _carritoFlujo.ObtenerPorID(CarritoId);

			if (resultado == null)
				return NotFound();

			return Ok(resultado);
		}

        [HttpPut("actualizar-total/{CarritoId}")]
        public async Task<IActionResult> ActualizarTotal(Guid CarritoId)
        {
            if (!await VerificarExistenciaCarrito(CarritoId))
                return NotFound("el carrito no existe");
            var resultado = await _carritoFlujo.ActualizarTotal(CarritoId);
            return NoContent();
        }
        private async Task<bool> VerificarExistenciaCarrito(Guid Id)
        {
            var ResultadoValidacion = false;
            var resultadoCategoriaExiste = await _carritoFlujo.ObtenerPorID(Id);
            if (resultadoCategoriaExiste != null)
                ResultadoValidacion = true;
            return ResultadoValidacion;
        }

        [HttpDelete("eliminar-total/{CarritoId}")]
        public async Task<IActionResult> EliminarTotal(Guid CarritoId)
        {
            if (!await VerificarExistenciaCarrito(CarritoId))
                return NotFound("el carrito no existe");
            var resultado = await _carritoFlujo.EliminarTotal(CarritoId);
            return NoContent();
        }


        [Authorize]
        [HttpGet("correo")]
        public async Task<IActionResult> ObtenerParaCorreo()
        {
            string idUsuarioStr = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "idUsuario")?.Value;

            if (string.IsNullOrEmpty(idUsuarioStr) || !Guid.TryParse(idUsuarioStr, out var usuarioId))
                return BadRequest("Usuario no válido.");

            var resultado = await _carritoFlujo.ObtenerParaCorreo(usuarioId);

            if (resultado == null)
                return NotFound("No se encontró un carrito para este usuario.");

            return Ok(resultado);
        }


        [HttpGet("enviar-correo")]
        public async Task<IActionResult> EnviarCorreoCarrito()
        {
            string idUsuarioStr = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "idUsuario")?.Value;

            if (string.IsNullOrEmpty(idUsuarioStr) || !Guid.TryParse(idUsuarioStr, out var usuarioId))
                return BadRequest("Usuario no válido.");

            var carrito = await _carritoFlujo.ObtenerParaCorreo(usuarioId);

            if (carrito == null)
                return NotFound("No se encontró un carrito para este usuario.");

            await _correoServicio.EnviarCorreoCarritoAsync(carrito);

            return Ok("Correo enviado correctamente.");
        }



    }
}
