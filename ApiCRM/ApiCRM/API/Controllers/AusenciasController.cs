using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using DA;
using Flujo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AusenciasController : ControllerBase, IAusenciasController
    {
        private readonly IAusenciasFlujo _ausenciasFlujo;
        private readonly ILogger<AusenciasController> _logger;
        public AusenciasController(IAusenciasFlujo ausenciasFlujo, ILogger<AusenciasController> logger)
        {
            _ausenciasFlujo = ausenciasFlujo;
            _logger = logger;
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] Ausencias ausencias)
        {
            var resultado = await _ausenciasFlujo.Agregar(ausencias);
            return CreatedAtAction(nameof(ObtenerPorId), new { AusenciasId = resultado }, null);
        }


        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _ausenciasFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [Authorize(Roles = "1")]
        [HttpGet("{AusenciasId}")]

        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid AusenciasId)
        {
            var resultado = await _ausenciasFlujo.ObtenerPorId(AusenciasId);
            return Ok(resultado);
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPut("{AusenciasId}")]
		public async Task<IActionResult> Editar([FromRoute] Guid AusenciasId, [FromBody] Ausencias ausencias)
		{
			/*if (!await VerificarExistenciaEmpleado(IdEmpleado))
				return NotFound("El empleado no esta registrado");*/
			var resultado = await _ausenciasFlujo.Editar(AusenciasId, ausencias);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
		[HttpPut("desactivar-ausencias/{AusenciasId}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid AusenciasId)
		{
			if (!await VerificarExistenciaEmpleado(AusenciasId))
				return NotFound("Bono no esta registrado");
			var resultado = await _ausenciasFlujo.Eliminar(AusenciasId);
            return Ok(resultado);
        }



		private async Task<bool> VerificarExistenciaEmpleado(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoExiste = await _ausenciasFlujo.ObtenerPorId(Id);
			if (resultadoExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}
	}
}
