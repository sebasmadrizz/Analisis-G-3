using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos;
using static Abstracciones.Modelos.Categorias;

namespace Abstracciones.Interfaces.Flujo
{
	public interface ICategoriasFlujo
	{
        Task<(IEnumerable<CategoriasResponse> categorias, int total)> ObtenerPaginado(int start, int length);
        Task<(IEnumerable<CategoriasResponse> categorias, int total, int filtradas, string sugerencia)>
     ObtenerCategoriasPaginadasAsync(int start, int length, string searchTerm);
        Task<IEnumerable<CategoriasResponse>> ObtenerPadres();
        Task<CategoriasResponse> ObtenerPorId(Guid IdCategoria);

		Task<Guid> AgregarPadre(CategoriasRequestPadre categorias);

        Task<Guid> AgregarHija(CategoriasRequestHija categorias);

        Task<Guid> Editar(Guid IdCategoria, CategoriasRequestPadre categorias);

        Task<Guid> Desactivar(Guid IdCategoria);
        Task<int> VerificarSiEsPadre(Guid IdCategoria);
        Task<IEnumerable<CategoriasResponse>> ObtenerHijas(Guid idPadre);
        Task<IEnumerable<CategoriasResponse>> ObtenerHijasRecursivo(Guid idPadre);

        Task <VerificarCategoriaResponse> ObtenerHijasTotales(Guid IdCategoria);

        Task<Guid> ActivarPadreHijas(Guid idCategoria, bool activarHijas);
        Task<Guid> ActivarHijas(Guid idCategoria);

        Task<(IEnumerable<CategoriasResponse> categorias, int total, int filtradas, string sugerencia)>
             BuscarCategoriasAsync(int start, int length, string searchTerm);
    }
}
