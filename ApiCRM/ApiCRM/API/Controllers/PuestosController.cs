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
    public class PuestosController : ControllerBase, IPuestosController
    {
        private readonly IPuestosFlujo _puestosFlujo;
        private readonly ILogger<PuestosController> _logger;
        public PuestosController(IPuestosFlujo puestosFlujo, ILogger<PuestosController> logger)
        {
            _puestosFlujo = puestosFlujo;
            _logger = logger;
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] Puestos puestos)
        {
            var resultado = await _puestosFlujo.Agregar(puestos);
            return CreatedAtAction(nameof(ObtenerPorId), new { PuestosId = resultado }, null);
        }


        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _puestosFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [Authorize(Roles = "1")]
        [HttpGet("{PuestosId}")]

        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid PuestosId)
        {
            var resultado = await _puestosFlujo.ObtenerPorId(PuestosId);
            return Ok(resultado);
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPut("{PuestosId}")]
		public async Task<IActionResult> Editar([FromRoute] Guid PuestosId, [FromBody] Puestos puestos)
		{
			/*if (!await VerificarExistenciaEmpleado(IdEmpleado))
				return NotFound("El empleado no esta registrado");*/
			var resultado = await _puestosFlujo.Editar(PuestosId, puestos);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
		[HttpPut("desactivar-puestos/{PuestosId}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid PuestosId)
		{
			if (!await VerificarExistenciaEmpleado(PuestosId))
				return NotFound("Puestos no esta registrado");
			var resultado = await _puestosFlujo.Eliminar(PuestosId);
            return Ok(resultado);
        }



		private async Task<bool> VerificarExistenciaEmpleado(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoExiste = await _puestosFlujo.ObtenerPorId(Id);
			if (resultadoExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}
	}
}
