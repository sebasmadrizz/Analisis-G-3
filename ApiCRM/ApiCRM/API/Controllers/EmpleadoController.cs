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
    public class EmpleadoController : ControllerBase, IEmpleadoController
    {
        private readonly IEmpleadoFlujo _empleadoFlujo;
        private readonly ILogger<EmpleadoController> _logger;
        public EmpleadoController(IEmpleadoFlujo empleadoFlujo, ILogger<EmpleadoController> logger)
        {
            _empleadoFlujo = empleadoFlujo;
            _logger = logger;
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] Empleado empleado)
        {
            var resultado = await _empleadoFlujo.Agregar(empleado);
            return CreatedAtAction(nameof(ObtenerPorId), new { IdEmpleado = resultado }, null);
        }


        [Authorize(Roles = "1")]
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _empleadoFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [Authorize(Roles = "1")]
        [HttpGet("{IdEmpleado}")]

        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid IdEmpleado)
        {
            var resultado = await _empleadoFlujo.ObtenerPorId(IdEmpleado);
            return Ok(resultado);
        }
        [Authorize(Roles = "1")]
        [HttpPut("{IdEmpleado}")]
		public async Task<IActionResult> Editar([FromRoute] Guid IdEmpleado, [FromBody] Empleado empleado)
		{
			if (!await VerificarExistenciaEmpleado(IdEmpleado))
				return NotFound("El empleado no esta registrado");
			var resultado = await _empleadoFlujo.Editar(IdEmpleado, empleado);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
		[HttpPut("desactivar-empleado/{IdEmpleado}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid IdEmpleado)
		{
			if (!await VerificarExistenciaEmpleado(IdEmpleado))
				return NotFound("El empleado no esta registrado");
			var resultado = await _empleadoFlujo.Eliminar(IdEmpleado);
            return Ok(resultado);
        }



		private async Task<bool> VerificarExistenciaEmpleado(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoVehiculoExiste = await _empleadoFlujo.ObtenerPorId(Id);
			if (resultadoVehiculoExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}
	}
}
