using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Servicios
{
    public class LevenshteinService
    {
        public int CalcularDistancia(string texto1, string texto2)
        {
            texto1 = Normalizar(texto1);
            texto2 = Normalizar(texto2);

            if (string.IsNullOrEmpty(texto1)) return texto2.Length;
            if (string.IsNullOrEmpty(texto2)) return texto1.Length;

            int n = texto1.Length;
            int m = texto2.Length;
            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int costo = texto1[i - 1] == texto2[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + costo
                    );
                }
            }

            return d[n, m];
        }

        /// <summary>
        /// Ranking genérico: recibe una lista de objetos y un selector de texto.
        /// </summary>
        public List<T> AplicarRanking<T>(IEnumerable<T> items, string searchTerm, Func<T, string> selector)
        {
            var normalizadoTerm = Normalizar(searchTerm);
            int threshold = CalcularUmbral(normalizadoTerm);

            return items
                .Select(item => new
                {
                    Item = item,
                    Texto = Normalizar(selector(item))
                })
                .Select(x => new
                {
                    x.Item,
                    Distancia = CalcularDistancia(x.Texto, normalizadoTerm),
                    IsPrefix = x.Texto.StartsWith(normalizadoTerm),
                    Texto = x.Texto
                })
                .Where(x => x.Distancia <= threshold || x.IsPrefix)
                .OrderByDescending(x => x.IsPrefix)
                .ThenBy(x => x.Distancia)
                .ThenBy(x => x.Texto)
                .Select(x => x.Item)
                .ToList();
        }

        /// <summary>
        /// Obtiene la mejor sugerencia genérica.
        /// </summary>
        public string ObtenerSugerencia<T>(IEnumerable<T> items, string searchTerm, Func<T, string> selector)
        {
            var normalizadoTerm = Normalizar(searchTerm);
            int threshold = CalcularUmbral(normalizadoTerm);

            var mejor = items
                .Select(item => new
                {
                    Texto = selector(item),
                    Normalizado = Normalizar(selector(item)),
                    Distancia = CalcularDistancia(Normalizar(selector(item)), normalizadoTerm),
                    IsPrefix = Normalizar(selector(item)).StartsWith(normalizadoTerm)
                })
                .Where(x => x.Distancia <= threshold || x.IsPrefix)
                .OrderByDescending(x => x.IsPrefix)
                .ThenBy(x => x.Distancia)
                .FirstOrDefault();

            return mejor?.Texto ?? "No existe";
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

        private int CalcularUmbral(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return 0;
            int len = term.Length;
            if (len <= 4) return 2;
            return Math.Min((int)Math.Ceiling(len * 0.25), 4);
        }
        public List<T> AplicarRankingMultipleCampos<T>(
    IEnumerable<T> items,
    string searchTerm,
    params Func<T, string>[] campos
)
        {
            if (campos == null || campos.Length == 0)
                throw new ArgumentException("Debe especificar al menos un campo para aplicar el ranking.");

            var normalizadoTerm = Normalizar(searchTerm);
            int threshold = CalcularUmbral(normalizadoTerm);

            var ranked = items
                .Select(item =>
                {
                    // Calcula distancia para todos los campos
                    var distancias = campos.Select(f => CalcularDistancia(Normalizar(f(item)), normalizadoTerm)).ToArray();
                    var isPrefix = campos.Any(f => Normalizar(f(item)).StartsWith(normalizadoTerm));

                    return new
                    {
                        Item = item,
                        DistanciaMin = distancias.Min(),
                        IsPrefix = isPrefix
                    };
                })
                .Where(x => x.DistanciaMin <= threshold || x.IsPrefix)
                .OrderByDescending(x => x.IsPrefix)
                .ThenBy(x => x.DistanciaMin)
                .Select(x => x.Item)
                .ToList();

            return ranked;
        }
    }
}
