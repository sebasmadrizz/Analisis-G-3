using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Servicios;

namespace Reglas
{
    public class ProductosRegla: IProductosRegla
    {
        private readonly IProductosDA _productosDA;
        private readonly LevenshteinService _levService;

        public ProductosRegla(IProductosDA productosDA, LevenshteinService levService)
        {
            _productosDA = productosDA;
            _levService = levService;
        }

        public async Task<(Paginacion<ProductosResponse> productos, int total, int filtradas, string sugerencia)> ObtenerProductosFTSApiAsync(int PageIndex, int PageSize, string searchTerm)
        {
            var (candidatos, total, filtradasSP, usaFallback) =
                await _productosDA.ObtenerProductosBuscadosFTS(PageIndex, PageSize, searchTerm);

            string sugerencia = "No existe";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var ranked = _levService.AplicarRankingMultipleCampos(
    candidatos.Items,
    searchTerm,
    c => c.Nombre,
    c => c.Marca,
    c => c.Categoria
);
                sugerencia = ranked.FirstOrDefault()?.Nombre ?? "No existe";

                if (usaFallback)
                {
                    var filtradas = ranked.Count;
                    candidatos.Items = ranked;
                    return (candidatos, total, filtradas, sugerencia);
                }
                else
                {
                    var filtradas = filtradasSP;
                    candidatos.Items = ranked;
                    return (candidatos, total, filtradas, sugerencia);
                }
            }

            return (candidatos, total, filtradasSP, sugerencia);
        }
    }
}
