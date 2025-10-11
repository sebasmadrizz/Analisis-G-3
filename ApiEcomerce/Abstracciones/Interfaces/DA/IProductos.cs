using Abstracciones.Modelos;
using static Abstracciones.Modelos.Categorias;

namespace Abstracciones.Interfaces.DA
{
    public interface IProductosDA
    {
        Task<IEnumerable<ProductosResponse>> ObtenerProductosIndex();
        Task<IEnumerable<ProductosResponse>> ObtenerProductosPorCategoria(Guid categoriaId);
        Task<(Paginacion<ProductosResponse> productos, int total, int filtradas, bool usaFallback)>
          ObtenerProductosBuscadosFTS(int PageIndex, int PageSize, string searchTerm);



        Task<IEnumerable<ProductosResponse>> Obtener();
       
        Task<ProductosResponse> ObtenerPorId(Guid IdProducto);

        Task<Guid> Agregar(ProductosRequest productos);

        Task<Guid> Editar(Guid IdProducto, ProductosRequest productos);

        Task<Guid> Eliminar(Guid IdProducto);
        Task<IEnumerable<ProductosResponse>> ObtenerProductosBuscados(string nombre);
        Task<Paginacion<ProductosResponse>> ObtenerProductosXCategoria(Guid idCategoria, int pageIndex, int pageSize);
        Task<Paginacion<ProductosResponse>> ListarProductosPaginado(int pageIndex, int pageSize);
    }
}
