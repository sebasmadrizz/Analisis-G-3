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
    public class IncapacidadesController : ControllerBase, IIncapacidadesController
    {
        private readonly IIncapacidadesFlujo _incapacidadesFlujo;
        private readonly ILogger<IncapacidadesController> _logger;
        public IncapacidadesController(IIncapacidadesFlujo incapacidadesFlujo, ILogger<IncapacidadesController> logger)
        {
            _incapacidadesFlujo = incapacidadesFlujo;
            _logger = logger;
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] Incapacidades incapacidad)
        {
            var resultado = await _incapacidadesFlujo.Agregar(incapacidad);
            return CreatedAtAction(nameof(ObtenerPorId), new { IncapacidadesId = resultado }, null);
        }


        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _incapacidadesFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [Authorize(Roles = "1")]
        [HttpGet("{IncapacidadesId}")]

        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid IncapacidadesId)
        {
            var resultado = await _incapacidadesFlujo.ObtenerPorId(IncapacidadesId);
            return Ok(resultado);
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPut("{IncapacidadesId}")]
		public async Task<IActionResult> Editar([FromRoute] Guid IncapacidadesId, [FromBody] Incapacidades incapacidad)
		{
			/*if (!await VerificarExistenciaEmpleado(IdEmpleado))
				return NotFound("El empleado no esta registrado");*/
			var resultado = await _incapacidadesFlujo.Editar(IncapacidadesId, incapacidad);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
		[HttpPut("desactivar-incapacidad/{IncapacidadesId}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid IncapacidadesId)
		{
			if (!await VerificarExistenciaEmpleado(IncapacidadesId))
				return NotFound("Incapacidad no esta registrado");
			var resultado = await _incapacidadesFlujo.Eliminar(IncapacidadesId);
            return Ok(resultado);
        }



		private async Task<bool> VerificarExistenciaEmpleado(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoExiste = await _incapacidadesFlujo.ObtenerPorId(Id);
			if (resultadoExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}
	}
}
