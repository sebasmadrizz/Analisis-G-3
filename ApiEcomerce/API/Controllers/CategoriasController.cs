using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using Flujo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Categorias;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
	public class CategoriasController : ControllerBase, ICategoriasController
	{
		private readonly ICategoriasFlujo _categoriasFlujo;
		private readonly ILogger<CategoriasController> _logger;

		public CategoriasController(ICategoriasFlujo categoriasFlujo, ILogger<CategoriasController> logger)
		{
			_categoriasFlujo = categoriasFlujo;
			_logger = logger;
		}
        [Authorize(Roles = "1")]
        [HttpPost("padre")]
        public async Task<IActionResult> AgregarPadre([FromBody] CategoriasRequestPadre categorias)
		{
			var resultado = await _categoriasFlujo.AgregarPadre(categorias);
			return CreatedAtAction(nameof(ObtenerPorId), new { IdCategoria = resultado }, null);

		}
        [Authorize(Roles = "1")]
        [HttpPost("hija")]
        public async Task<IActionResult> AgregarHija([FromBody] CategoriasRequestHija categorias)
        {
            var resultado = await _categoriasFlujo.AgregarHija(categorias);
            return CreatedAtAction(nameof(ObtenerPorId), new { IdCategoria = resultado }, null);

        }


        [Authorize(Roles = "1")]
        [HttpPut("editar/{IdCategoria}")]
        public async Task<IActionResult> Editar([FromRoute] Guid IdCategoria, [FromBody] CategoriasRequestPadre categoria)
		{
			if (!await VerificarExistenciaCategoria(IdCategoria))
				return NotFound("la categoria no existe");
			var resultado = await _categoriasFlujo.Editar(IdCategoria, categoria);
			return Ok(resultado);
		}

        [Authorize(Roles = "1")]
        [HttpGet("Verificar/{IdCategoria}")]
        public async Task<IActionResult> VerificarCategoria([FromRoute] Guid IdCategoria)
        {
            if (!await VerificarExistenciaCategoria(IdCategoria))
                return NotFound("La categoría no existe.");

            int cantidadHijas = await _categoriasFlujo.VerificarSiEsPadre(IdCategoria);
            bool esPadre = cantidadHijas > 0;

            return Ok(new
            {
                IdCategoria = IdCategoria,
                EsPadre = esPadre,
                CantidadHijas = cantidadHijas
            });
        }

        [Authorize(Roles = "1")]
        [HttpPut("Desactivar/{IdCategoria}")]
        public async Task<IActionResult> Desactivar([FromRoute] Guid IdCategoria)
        {
            if (!await VerificarExistenciaCategoria(IdCategoria))
                return NotFound("La categoría no existe.");
            var resultado = await _categoriasFlujo.Desactivar(IdCategoria);
            return Ok(resultado);
        }


        [AllowAnonymous]
        [HttpGet("paginado-categorias")]
        public async Task<IActionResult> ObtenerCategoriasPaginado(
     [FromQuery] int start,
     [FromQuery] int length,
     [FromQuery] int draw)
        {
            // Asignamos defaults dentro del método si es necesario
            start = start < 0 ? 0 : start;
            length = length <= 0 ? 10 : length;
            draw = draw <= 0 ? 1 : draw;

            var (categorias, total) = await _categoriasFlujo.ObtenerPaginado(start, length);

            if (!categorias.Any())
                return NoContent();

            return Ok(new
            {
                draw,
                recordsTotal = total,
                recordsFiltered = total,
                data = categorias
            });
        }


        [AllowAnonymous]
        [HttpGet("{IdCategoria}")]
		public async Task<IActionResult> ObtenerPorId([FromRoute] Guid IdCategoria)
		{
			var resultado = await _categoriasFlujo.ObtenerPorId(IdCategoria);
			return Ok(resultado);
		}

        private async Task<bool> VerificarExistenciaCategoria(Guid Id)
		{
			var ResultadoValidacion = false;
			var resultadoCategoriaExiste = await _categoriasFlujo.ObtenerPorId(Id);
			if (resultadoCategoriaExiste != null)
				ResultadoValidacion = true;
			return ResultadoValidacion;
		}

        [AllowAnonymous]
        [HttpGet("hijas/{idPadre}")]
        public async Task<IActionResult> ObtenerHijas(Guid idPadre)
        {
            var resultado = await _categoriasFlujo.ObtenerHijas(idPadre);
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }
        [AllowAnonymous]
        [HttpGet("hijas-recursivo/{idPadre}")]
        public async Task<IActionResult> ObtenerHijasRecursivo(Guid idPadre)
        {
            var resultado = await _categoriasFlujo.ObtenerHijasRecursivo(idPadre);
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("padres")]
        public async  Task<IActionResult> ObtenerPadres()
        {
            var resultado = await _categoriasFlujo.ObtenerPadres();
            if (!resultado.Any())
                return NoContent();
            return Ok(resultado);
        }

        [AllowAnonymous]
        [HttpGet("hijas-totales/{idPadre}")]
        public async Task<IActionResult> ObtenerHijasTotales(Guid idPadre)
        {
            if (!await VerificarExistenciaCategoria(idPadre))
                return NotFound("La categoría no existe.");
            var resultado = await _categoriasFlujo.ObtenerHijasTotales(idPadre);
            return Ok(resultado);
        }

        [Authorize(Roles = "1")]
        [HttpPut("Activar-padre/{idCategoria}")]
        public async Task<IActionResult> ActivarPadreHijas(Guid idCategoria, bool activarHijas)
        {
            if (!await VerificarExistenciaCategoria(idCategoria))
                return NotFound("la categoria no existe");
            var resultado = await _categoriasFlujo.ActivarPadreHijas(idCategoria, activarHijas);
            return Ok(resultado);
        }
        [Authorize(Roles = "1")]
        [HttpPut("Activar-hijas/{idCategoria}")]
        public async Task<IActionResult> ActivarHijas(Guid idCategoria)
        {
            if (!await VerificarExistenciaCategoria(idCategoria))
                return NotFound("la categoria no existe");
            var resultado = await _categoriasFlujo.ActivarHijas(idCategoria);
            return Ok(resultado);
        }




        [AllowAnonymous]
        [HttpGet("paginado-categorias-fts")]
        public async Task<IActionResult> ObtenerCategoriasPaginadoBusqueda(
            [FromQuery] int start,
            [FromQuery] int length,
            [FromQuery] int draw,
            [FromQuery] string searchTerm)
        {
            // Validaciones básicas
            start = Math.Max(start, 0);
            length = length <= 0 ? 10 : length;
            draw = draw <= 0 ? 1 : draw;

            // Llamada al flujo/servicio
            var (categorias, total, filtradas, sugerencia) =
                await _categoriasFlujo.ObtenerCategoriasPaginadasAsync(start, length, searchTerm);

            if (!categorias.Any())
                return NoContent();

            // Devolvemos resultado compatible con DataTables server-side
            return Ok(new
            {
                draw,
                recordsTotal = total,          // total de filas en la tabla
                recordsFiltered = filtradas,   // total filtrado según searchTerm
                data = categorias,
                suggestion = sugerencia        // opcional: puedes usarlo en la UI
            });
        }


        [AllowAnonymous]
        [HttpGet("busqueda-api")]
        public async Task<IActionResult> BusquedaCategoriasApi(
     [FromQuery] int start,
     [FromQuery] int length,
     [FromQuery] int draw,
     [FromQuery] string searchTerm)
        {
            // Validaciones básicas
            start = Math.Max(start, 0);
            length = length <= 0 ? 10 : length;
            draw = draw <= 0 ? 1 : draw;

            // 1️⃣ Llamada al flujo (nombre correcto del método)
            var (categorias, total, filtradas, sugerencia) =
                await _categoriasFlujo.BuscarCategoriasAsync(start, length, searchTerm);

            // 2️⃣ Siempre devolver objeto aunque no haya resultados
            return Ok(new
            {
                draw,
                recordsTotal = total,           // total de filas en la tabla
                recordsFiltered = filtradas,    // total filtrado según searchTerm
                data = categorias,              // lista paginada
                suggestion = sugerencia         // sugerencia de búsqueda
            });
        }

    }
}
