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
    public class HorasExtrasController : ControllerBase, IHorasExtraController
    {
        private readonly IHorasExtrasFlujo _horasExtrasFlujo;
        private readonly ILogger<HorasExtrasController> _logger;
        public HorasExtrasController(IHorasExtrasFlujo horasExtrasFlujo, ILogger<HorasExtrasController> logger)
        {
            _horasExtrasFlujo = horasExtrasFlujo;
            _logger = logger;
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] HorasExtras horasExtras)
        {
            var resultado = await _horasExtrasFlujo.Agregar(horasExtras);
            return CreatedAtAction(nameof(ObtenerPorId), new { HorasExtrasId = resultado }, null);
        }


        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _horasExtrasFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [Authorize(Roles = "1")]
        [HttpGet("{HorasExtrasId}")]

        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid HorasExtrasId)
        {
            var resultado = await _horasExtrasFlujo.ObtenerPorId(HorasExtrasId);
            return Ok(resultado);
        }
        [AllowAnonymous]//cambiar a Authorize(Roles="1") para produccion
        [HttpPut("{HorasExtrasId}")]
		public async Task<IActionResult> Editar([FromRoute] Guid HorasExtrasId, [FromBody] HorasExtras horasExtras)
		{
			/*if (!await VerificarExistenciaEmpleado(IdEmpleado))
				return NotFound("El empleado no esta registrado");*/
			var resultado = await _horasExtrasFlujo.Editar(HorasExtrasId, horasExtras);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
		[HttpPut("desactivar-horasExtras/{HorasExtrasId}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid HorasExtrasId)
		{
			if (!await VerificarExistenciaEmpleado(HorasExtrasId))
				return NotFound("Horas extras no esta registrado");
			var resultado = await _horasExtrasFlujo.Eliminar(HorasExtrasId);
            return Ok(resultado);
        }



		private async Task<bool> VerificarExistenciaEmpleado(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoExiste = await _horasExtrasFlujo.ObtenerPorId(Id);
			if (resultadoExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}
	}
}
