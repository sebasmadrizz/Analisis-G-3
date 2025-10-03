using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IProductosFlujo
    {
        
        Task<IEnumerable<ProductosResponse>> Obtener();
        Task<IEnumerable<ProductosResponse>> ObtenerProductosBuscados(string nombre);
        Task<Paginacion<ProductosResponse>> ObtenerProductosXCategoria(Guid idCategoria, int pageIndex, int pageSize);
        Task<ProductosResponse> ObtenerPorId(Guid IdProducto);

        Task<Guid> Agregar(ProductosRequest productos,Documento imagen);

        Task<Guid> Editar(Guid IdProducto, ProductosRequest productos, Documento imagen);

        Task<Guid> Eliminar(Guid IdProducto);
        Task<Paginacion<ProductosResponse>> ListarProductosPaginado(int pageIndex, int pageSize);
    }
}
