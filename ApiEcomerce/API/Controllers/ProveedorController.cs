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
    public class ProveedorController : ControllerBase, IProveedorController
    {
        private readonly IProveedorFlujo _proveedorFlujo;
        private readonly ILogger<ProveedorController> _logger;
        public ProveedorController(IProveedorFlujo proveedorFlujo, ILogger<ProveedorController> logger)
        {
            _proveedorFlujo = proveedorFlujo;
            _logger = logger;
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] Proveedores proveedor)
        {
            var resultado = await _proveedorFlujo.Agregar(proveedor);
            return CreatedAtAction(nameof(ObtenerPorId), new { IdProveedor = resultado }, null);
        }


        [AllowAnonymous]
        [HttpGet]

        public async Task<IActionResult> Obtener()
        {
            var resultado = await _proveedorFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }


        [Authorize(Roles = "1")]
        [HttpGet("{IdProveedor}")]

        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid IdProveedor)
        {
            var resultado = await _proveedorFlujo.ObtenerPorId(IdProveedor);
            return Ok(resultado);
        }
        [Authorize(Roles = "1")]
        [HttpPut("{IdProveedor}")]
		public async Task<IActionResult> Editar([FromRoute] Guid IdProveedor, [FromBody] Proveedores proveedor)
		{
			if (!await VerificarExistenciaProveedor(IdProveedor))
				return NotFound("El proveedor no esta registrado");
			var resultado = await _proveedorFlujo.Editar(IdProveedor, proveedor);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
		[HttpPut("desactivar-proveedor/{IdProveedor}")]
		public async Task<IActionResult> Eliminar([FromRoute] Guid IdProveedor)
		{
			if (!await VerificarExistenciaProveedor(IdProveedor))
				return NotFound("El proveedor no esta registrado");
			var resultado = await _proveedorFlujo.Eliminar(IdProveedor);
            return Ok(resultado);
        }



		private async Task<bool> VerificarExistenciaProveedor(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoVehiculoExiste = await _proveedorFlujo.ObtenerPorId(Id);
			if (resultadoVehiculoExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}
	}
}
