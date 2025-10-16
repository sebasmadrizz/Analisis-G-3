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
    public class BonosController : ControllerBase, IBonosController
    {
        private readonly IBonosFlujo _bonosFlujo;
        private readonly ILogger<BonosController> _logger;
        public BonosController(IBonosFlujo bonosFlujo, ILogger<BonosController> logger)
        {
            _bonosFlujo = bonosFlujo;
            _logger = logger;
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] Bonos bonos)
        {
            var resultado = await _bonosFlujo.Agregar(bonos);
            return CreatedAtAction(nameof(ObtenerPorId), new { BonosId = resultado }, null);
        }


        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _bonosFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [Authorize(Roles = "1")]
        [HttpGet("{BonosId}")]

        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid BonosId)
        {
            var resultado = await _bonosFlujo.ObtenerPorId(BonosId);
            return Ok(resultado);
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPut("{BonosId}")]
		public async Task<IActionResult> Editar([FromRoute] Guid BonosId, [FromBody] Bonos bonos)
		{
			/*if (!await VerificarExistenciaEmpleado(IdEmpleado))
				return NotFound("El empleado no esta registrado");*/
			var resultado = await _bonosFlujo.Editar(BonosId, bonos);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
		[HttpPut("desactivar-bonos/{BonosId}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid BonosId)
		{
			if (!await VerificarExistenciaEmpleado(BonosId))
				return NotFound("Bono no esta registrado");
			var resultado = await _bonosFlujo.Eliminar(BonosId);
            return Ok(resultado);
        }



		private async Task<bool> VerificarExistenciaEmpleado(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoExiste = await _bonosFlujo.ObtenerPorId(Id);
			if (resultadoExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}
	}
}
