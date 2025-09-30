using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos;
using static Abstracciones.Modelos.Categorias;

namespace Abstracciones.Interfaces.Reglas
{
    public interface ICategoriasReglas
    {
        /// <summary>
        /// Obtiene todas las categorías hijas recursivamente a partir de una lista de categorías y un IdPadre.
        /// </summary>
        /// <param name="todasCategorias">Lista completa de categorías.</param>
        /// <param name="idPadre">Id de la categoría padre.</param>
        /// <returns>Lista de todas las categorías hijas (directas e indirectas).</returns>
        IEnumerable<CategoriasResponse> ObtenerHijasRecursivo(IEnumerable<CategoriasResponse> todasCategorias, Guid idPadre);

        Task<(IEnumerable<CategoriasResponse> categorias, int total, int filtradas, string sugerencia)>
        ObtenerCategoriasApiAsync(int start, int length, string searchTerm);

    }
}
