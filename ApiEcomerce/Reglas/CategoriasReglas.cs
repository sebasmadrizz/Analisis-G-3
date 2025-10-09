using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Servicios;
using static Abstracciones.Modelos.Categorias;

namespace Reglas
{
    public class CategoriasReglas : ICategoriasReglas

    {
        private readonly ICategoriasDA _categoriasDA;
        private readonly LevenshteinService _levService;

        public CategoriasReglas(ICategoriasDA categoriasDA, LevenshteinService levService)
        {
            _categoriasDA = categoriasDA;
            _levService = levService;
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



        public async Task<(IEnumerable<CategoriasResponse> categorias, int total, int filtradas, string sugerencia)>
      ObtenerCategoriasApiAsync(int start, int length, string searchTerm)
        {
            var (candidatos, total, filtradasSP, usaFallback) =
                await _categoriasDA.ObtenerCategoriasApiAsync(start, length, searchTerm);

            string sugerencia = "No existe";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var ranked = _levService.AplicarRanking(candidatos, searchTerm, c => c.Nombre).ToList();
                sugerencia = ranked.FirstOrDefault()?.Nombre ?? "No existe";

                if (usaFallback)
                {
                    var filtradas = ranked.Count;
                    var paginados = ranked.Skip(start).Take(length);
                    return (paginados, total, filtradas, sugerencia);
                }
                else
                {
                    var filtradas = filtradasSP;
                    var paginados = ranked;
                    return (paginados, total, filtradas, sugerencia);
                }
            }

            return (candidatos, total, filtradasSP, sugerencia);
        }






        private string Normalizar(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "";

            var normalized = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC).ToUpperInvariant();
        }


    }
}