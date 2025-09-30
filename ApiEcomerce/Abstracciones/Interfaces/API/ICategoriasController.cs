using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Categorias;

namespace Abstracciones.Interfaces.API
{
	public interface ICategoriasController
	{
        Task<IActionResult> ObtenerCategoriasPaginado(int start, int length, int draw);

        Task<IActionResult> ObtenerCategoriasPaginadoBusqueda(int start, int length, int draw, string searchTerm);

        Task<IActionResult> ObtenerPadres();
        Task<IActionResult> ObtenerPorId(Guid IdCategoria);

		Task<IActionResult> AgregarPadre(CategoriasRequestPadre categorias);

        Task<IActionResult> AgregarHija(CategoriasRequestHija categorias);
        Task<IActionResult> Editar(Guid IdCategoria, CategoriasRequestPadre categorias);
        Task<IActionResult> Desactivar(Guid IdCategoria);
        Task<IActionResult> ObtenerHijas(Guid idPadre);
        Task<IActionResult> ObtenerHijasRecursivo(Guid idPadre);

        Task<IActionResult> VerificarCategoria(Guid IdCategoria);


        Task<IActionResult> ObtenerHijasTotales(Guid idPadre);

        Task<IActionResult> ActivarPadreHijas(Guid idCategoria, bool activarHijas);

        Task<IActionResult> ActivarHijas(Guid idCategoria);



    }
}
