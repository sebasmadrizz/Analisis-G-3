using Abstracciones.Modelos;
using static Abstracciones.Modelos.Categorias;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IProductosRegla
    {
        Task<(Paginacion<ProductosResponse> productos, int total, int filtradas, string sugerencia)>
        ObtenerProductosFTSApiAsync(int PageIndex, int PageSize, string searchTerm);
    }
}
