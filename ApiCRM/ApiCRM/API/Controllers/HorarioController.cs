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
    public class HorarioController : ControllerBase, IHorarioController
    {
        private readonly IHorarioFlujo _horarioFlujo;
        private readonly ILogger<HorarioController> _logger;
        public HorarioController(IHorarioFlujo horarioFlujo, ILogger<HorarioController> logger)
        {
            _horarioFlujo = horarioFlujo;
            _logger = logger;
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] Horario horario)
        {
            var resultado = await _horarioFlujo.Agregar(horario);
            return CreatedAtAction(nameof(ObtenerPorId), new { HorarioId = resultado }, null);
        }


        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _horarioFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [Authorize(Roles = "1")]
        [HttpGet("{HorarioId}")]

        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid HorarioId)
        {
            var resultado = await _horarioFlujo.ObtenerPorId(HorarioId);
            return Ok(resultado);
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPut("{HorarioId}")]
		public async Task<IActionResult> Editar([FromRoute] Guid HorarioId, [FromBody] Horario horario)
		{
			/*if (!await VerificarExistenciaEmpleado(IdEmpleado))
				return NotFound("El empleado no esta registrado");*/
			var resultado = await _horarioFlujo.Editar(HorarioId, horario);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
		[HttpPut("desactivar-horario/{HorarioId}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid HorarioId)
		{
			if (!await VerificarExistenciaEmpleado(HorarioId))
				return NotFound("Bono no esta registrado");
			var resultado = await _horarioFlujo.Eliminar(HorarioId);
            return Ok(resultado);
        }



		private async Task<bool> VerificarExistenciaEmpleado(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoExiste = await _horarioFlujo.ObtenerPorId(Id);
			if (resultadoExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}
	}
}
