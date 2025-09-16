using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using static Abstracciones.Modelos.Categorias;

namespace Reglas
{
    public class CategoriasReglas : ICategoriasReglas

    {
        private readonly ICategoriasDA _categoriasDA;

        public CategoriasReglas(ICategoriasDA categoriasDA)
        {
            _categoriasDA = categoriasDA;
        }

       
        public IEnumerable<CategoriasResponse> ObtenerHijasRecursivo(IEnumerable<CategoriasResponse> todasCategorias, Guid idPadre)
        {
            var resultado = new List<CategoriasResponse>();
            var visitados = new HashSet<Guid>();

            void ObtenerHijasInterno(Guid idActual)
            {
                if (visitados.Contains(idActual)) return;
                visitados.Add(idActual);

                var hijasDirectas = todasCategorias.Where(c => c.PadreId == idActual).ToList();
                foreach (var hija in hijasDirectas)
                {
                    resultado.Add(hija);
                    ObtenerHijasInterno(hija.CategoriasId);
                }
            }

            ObtenerHijasInterno(idPadre);
            return resultado;
        }
    }
}