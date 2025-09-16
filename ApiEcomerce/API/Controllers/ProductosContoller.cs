using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase, IProductosController
    {
        private readonly IProductosFlujo _productosFlujo;
        private readonly ILogger<ProductosController> _logger;

        public ProductosController(IProductosFlujo productosFlujo, ILogger<ProductosController> logger)
        {
            _productosFlujo = productosFlujo;
            _logger = logger;
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public async Task<IActionResult> Agregar([FromBody] ProductoConImagenRequest request)
        {
            var resultado = await _productosFlujo.Agregar(request.Productos, request.Imagen);
            return CreatedAtAction(nameof(ObtenerPorId), new { IdProducto = resultado }, null); 

        }
        [Authorize(Roles = "1")]
        [HttpPut("{IdProducto}")]
        public async Task<IActionResult> Editar([FromRoute] Guid IdProducto, [FromBody] ProductoConImagenRequest request)
        {
            if (!await VerificarProductosExiste(IdProducto))
                return NotFound("el producto no existe");
            var resultado = await _productosFlujo.Editar(IdProducto, request.Productos, request.Imagen);
            return Ok(resultado);
        }
        [Authorize(Roles = "1")]
        [HttpPut("estados-producto/{IdProducto}")]
        public async Task<IActionResult> Eliminar([FromRoute] Guid IdProducto)
        {
            if (!await VerificarProductosExiste(IdProducto))
                return NotFound("el producto no existe");
            var resultado = await _productosFlujo.Eliminar(IdProducto);
            return Ok(resultado);
        }
        [AllowAnonymous]
        [HttpGet("ProductosPaginados/{pageIndex}/{pageSize}")]

        public async Task<IActionResult> ListarProductosPaginado([FromRoute] int pageIndex, [FromRoute]  int pageSize)
        {
            var resultado = await _productosFlujo.ListarProductosPaginado(pageIndex, pageSize);
            return Ok(resultado);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var resultado = await _productosFlujo.Obtener();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }
        [AllowAnonymous]
        [HttpGet("{IdProducto}")]
        public async Task<IActionResult> ObtenerPorId([FromRoute] Guid IdProducto)
        {
            var resultado = await _productosFlujo.ObtenerPorId(IdProducto);
            return Ok(resultado);
        }
        [AllowAnonymous]
        [HttpGet("Busqueda/{nombre}")]
        public async Task<IActionResult> ObtenerProductosBuscados([FromRoute] string nombre)
        {
            var resultado = await _productosFlujo.ObtenerProductosBuscados(nombre);
            return Ok(resultado);
        }
        [AllowAnonymous]
        [HttpGet("ProductosXCategoria/{idCategoria}/{pageIndex}/{pageSize}")]

        public async Task<IActionResult> ObtenerProductosXCategoria([FromRoute] Guid idCategoria, [FromRoute] int pageIndex, [FromRoute] int pageSize)
        {
            var resultado = await _productosFlujo.ObtenerProductosXCategoria(idCategoria, pageIndex, pageSize);
            return Ok(resultado);
        }

        private async Task<bool> VerificarProductosExiste(Guid Id)
        {
            var ResultadoValidacion = false;
            var resultadoVehiculoExiste = await _productosFlujo.ObtenerPorId(Id);
            if (resultadoVehiculoExiste != null)
                ResultadoValidacion = true;
            return ResultadoValidacion;
        }
    }
}
