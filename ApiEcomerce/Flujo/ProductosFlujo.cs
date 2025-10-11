using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Servicios;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Flujo
{
    public class ProductosFlujo : IProductosFlujo
    {
        private readonly IProductosDA _productosDA;
        private readonly IDocumentoRegla _documentoRegla;
        private readonly IProductosRegla _productosReglas;
        private readonly LevenshteinService _levService;


        public ProductosFlujo(IProductosRegla productosReglas,LevenshteinService levService,IProductosDA productosDA, IDocumentoRegla documentoRegla)
        {
            _productosDA = productosDA;
            _documentoRegla =documentoRegla;
            _levService = levService;
            _productosReglas=productosReglas;

        }
        public async Task<Guid> Agregar(ProductosRequest productos, Documento imagen)
        {
            var imagenUrl = await _documentoRegla.GuardarDocumento(imagen);
            productos.ImagenUrl = imagenUrl;
            return await _productosDA.Agregar(productos);
        }

        public async Task<Guid> Editar(Guid IdProducto, ProductosRequest productos, Documento imagen)
        {
            var imagenUrl = await _documentoRegla.GuardarDocumentoEditar(imagen,productos.ImagenUrl);
            productos.ImagenUrl = imagenUrl;
            return await _productosDA.Editar(IdProducto, productos);
        }

        public async Task<Guid> Eliminar(Guid IdProducto)
        {
            return await _productosDA.Eliminar(IdProducto);
        }

        public Task<Paginacion<ProductosResponse>> ListarProductosPaginado(int pageIndex, int pageSize)
        {
            return _productosDA.ListarProductosPaginado(pageIndex, pageSize);
        }

        public async Task<IEnumerable<ProductosResponse>> Obtener()
        {
            return await _productosDA.Obtener();
        }

        public async Task<ProductosResponse> ObtenerPorId(Guid IdProducto)
        {
            return await _productosDA.ObtenerPorId(IdProducto);
        }

        public async Task<IEnumerable<ProductosResponse>> ObtenerProductosBuscados(string nombre)
        {
            return await _productosDA.ObtenerProductosBuscados(nombre);
        }

        public async Task<(Paginacion<ProductosResponse> productos, int total, int filtradas, string sugerencia)> 
            ObtenerProductosBuscadosFTS(int PageIndex, int PageSize, string searchTerm)
        {
            return await _productosReglas.ObtenerProductosFTSApiAsync(PageIndex, PageSize, searchTerm);
        }

        public async Task<IEnumerable<ProductosResponse>> ObtenerProductosIndex()
        {
            return await _productosDA.ObtenerProductosIndex();
        }

        public async Task<IEnumerable<ProductosResponse>> ObtenerProductosPorCategoria(Guid categoriaId)
        {
            return await _productosDA.ObtenerProductosPorCategoria(categoriaId);
        }

        public async Task<Paginacion<ProductosResponse>> ObtenerProductosXCategoria(Guid idCategoria, int pageIndex, int pageSize)
        {
            return await _productosDA.ObtenerProductosXCategoria(idCategoria, pageIndex, pageSize);
        }

        
    }
}
