using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reglas;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase, IProductosController
    {
        private readonly IProductosFlujo _productosFlujo;
        private readonly ILogger<ProductosController> _logger;
        private readonly IExportarArchivosReglas _exportarArchivosReglas;

        public ProductosController(IExportarArchivosReglas exportarArchivosReglas, IProductosFlujo productosFlujo, ILogger<ProductosController> logger)
        {
            _productosFlujo = productosFlujo;
            _logger = logger;
            _exportarArchivosReglas = exportarArchivosReglas;
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
        [Authorize(Roles = "1")]
        [HttpGet("ExportarArchivoExel")]

        public async Task<IActionResult> ExportExel()
        {
            var productos = await _productosFlujo.Obtener();
            var excelBytes = _exportarArchivosReglas.ExportExel(productos);
            return File(
            excelBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Inventario.xlsx"
        );
        }

        private async Task<bool> VerificarProductosExiste(Guid Id)
        {
            var ResultadoValidacion = false;
            var resultadoVehiculoExiste = await _productosFlujo.ObtenerPorId(Id);
            if (resultadoVehiculoExiste != null)
                ResultadoValidacion = true;
            return ResultadoValidacion;
        }
        [Authorize(Roles = "1")]
        [HttpGet("ExportarArchivoPDF")]

        public async Task<IActionResult> ExportPdf()
        {
            var productos = await _productosFlujo.Obtener();
            var pdfBytes = _exportarArchivosReglas.ExportPdf(productos);
            return File(
        pdfBytes,
        "application/pdf",   
        "Inventario.pdf"     
    );
        }
    }
}
