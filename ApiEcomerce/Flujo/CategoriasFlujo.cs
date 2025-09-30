using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using static Abstracciones.Modelos.Categorias;
using Servicios;


namespace Flujo
{
	public class CategoriasFlujo : ICategoriasFlujo
	{
		private readonly ICategoriasDA _categoriasDA;
        private readonly ICategoriasReglas _categoriaReglas;
        private readonly LevenshteinService _levService;


        public CategoriasFlujo(ICategoriasDA categoriasDA, ICategoriasReglas categoriasReglas, LevenshteinService levService)
        {
            _categoriasDA = categoriasDA ;
            _categoriaReglas = categoriasReglas;
            _levService = levService;
        }

        public async Task<Guid> AgregarHija(CategoriasRequestHija categorias)
        {
            return await _categoriasDA.AgregarHija(categorias);
        }
        public async Task<IEnumerable<CategoriasResponse>> Obtener()
        {
            return await _categoriasDA.Obtener();
        }

        public async Task<Guid> AgregarPadre(CategoriasRequestPadre categorias)
        {
            return await _categoriasDA.AgregarPadre(categorias);
        }

        public async Task<Guid> Editar(Guid IdCategoria, CategoriasRequestPadre categorias)
        {
            return await _categoriasDA.Editar(IdCategoria, categorias);
        }

        public async Task<Guid> Desactivar(Guid IdCategoria)
        {
            return await _categoriasDA.Desactivar(IdCategoria);
        }

        public async Task<(IEnumerable<CategoriasResponse> categorias, int total)> ObtenerPaginado(int start, int length)
        {
            return await _categoriasDA.ObtenerPaginado(start, length);
        }

        public async Task<CategoriasResponse> ObtenerPorId(Guid IdCategoria)
        {
            return await _categoriasDA.ObtenerPorId(IdCategoria);
        }

        public async Task<IEnumerable<CategoriasResponse>> ObtenerHijas(Guid idPadre)
        {
            return await _categoriasDA.ObtenerHijas(idPadre);
        }

     /*  public async Task<IEnumerable<CategoriasResponse>> ObtenerHijasRecursivo(Guid idPadre)
        {
            var categorias = await _categoriasDA.Obtener();
            return _categoriaReglas.ObtenerHijasRecursivo(categorias, idPadre);
        }*/

        public async Task<IEnumerable<CategoriasResponse>> ObtenerPadres()
        {
            return await _categoriasDA.ObtenerPadres();
        }

        public async  Task<int> VerificarSiEsPadre(Guid IdCategoria)
        {
            return await _categoriasDA.TieneHijas(IdCategoria);
        }

       

        public async Task<Guid> ActivarPadreHijas(Guid idCategoria, bool activarHijas)
        {
            return await _categoriasDA.ActivarPadreHijas(idCategoria, activarHijas);
        }

        public async Task<VerificarCategoriaResponse> ObtenerHijasTotales(Guid IdCategoria)
        {
            return await _categoriasDA.ObtenerHijasTotales(IdCategoria);
        }

        public async Task<Guid> ActivarHijas(Guid idCategoria)
        {
            return await _categoriasDA.ActivarHijas(idCategoria);
        }

        public Task<IEnumerable<CategoriasResponse>> ObtenerHijasRecursivo(Guid idPadre)
        {
            throw new NotImplementedException();
        }

        public async Task<(IEnumerable<CategoriasResponse> categorias, int total, int filtradas, string sugerencia)>
         ObtenerCategoriasPaginadasAsync(int start, int length, string searchTerm)
        {
            var result = await _categoriasDA.ObtenerPaginadoBusquedaAsync(start, length, searchTerm);
            return result;
        }



        public async Task<(IEnumerable<CategoriasResponse> categorias, int total, int filtradas, string sugerencia)>
    BuscarCategoriasAsync(int start, int length, string searchTerm)
        {
            return await _categoriaReglas.ObtenerCategoriasApiAsync(start, length, searchTerm);
        }

        public async Task<IEnumerable<CategoriaPadreConHijas>> ObtenerCategoriaPadreConHijas()
        {
            return await _categoriasDA.ObtenerCategoriaPadreConHijas();
        }
    }

}