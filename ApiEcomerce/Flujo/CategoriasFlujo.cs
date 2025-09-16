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

namespace Flujo
{
	public class CategoriasFlujo : ICategoriasFlujo
	{
		private readonly ICategoriasDA _categoriasDA;
        private readonly ICategoriasReglas _categoriaReglas;

        public CategoriasFlujo(ICategoriasDA categoriasDA, ICategoriasReglas categoriasReglas)
		{
			_categoriasDA = categoriasDA;
            _categoriaReglas = categoriasReglas;

        }

        public async Task<Guid> AgregarHija(CategoriasRequestHija categorias)
        {
            return await _categoriasDA.AgregarHija(categorias);
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

        public async Task<IEnumerable<CategoriasResponse>> Obtener()
        {
            return await _categoriasDA.Obtener();
        }

        public async Task<CategoriasResponse> ObtenerPorId(Guid IdCategoria)
        {
            return await _categoriasDA.ObtenerPorId(IdCategoria);
        }

        public async Task<IEnumerable<CategoriasResponse>> ObtenerHijas(Guid idPadre)
        {
            return await _categoriasDA.ObtenerHijas(idPadre);
        }

        public async Task<IEnumerable<CategoriasResponse>> ObtenerHijasRecursivo(Guid idPadre)
        {
            var categorias = await _categoriasDA.Obtener();
            return _categoriaReglas.ObtenerHijasRecursivo(categorias, idPadre);
        }

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
    }

}