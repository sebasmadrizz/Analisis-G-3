using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ProductosDA : IProductosDA
    {
        
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;


        public ProductosDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }


        public async Task<Guid> Agregar(ProductosRequest productos)
        {
            string query = @"AGREGAR_PRODUCTO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdProducto = Guid.NewGuid(),
                IdProveedor = productos.IdProveedor,
                Marca = productos.Marca,
                IdCategoria = productos.IdCategoria,
                IdEstado = 1,
                Nombre = productos.Nombre,
                Precio = productos.Precio,
                Descripcion = productos.Descripcion,
                Stock = productos.Stock,
                ImagenUrl = productos.ImagenUrl,
                FechaCreacion = DateTime.Now
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid IdProducto, ProductosRequest productos)
        {
            await VerificarProductoExiste(IdProducto);


            string query = @"EDITAR_PRODUCTO";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdProducto = IdProducto,
                Nombre = productos.Nombre,
                Marca = productos.Marca,
                Precio = productos.Precio,
                Descripcion = productos.Descripcion,
                Stock = productos.Stock,
                ImagenUrl = productos.ImagenUrl,
                ProveedorID=productos.IdProveedor,
                CategoriasId=productos.IdCategoria


            });

            return resultado;
        }

        public async Task<Guid> Eliminar(Guid IdProducto)
        {
            await VerificarProductoExiste(IdProducto);
            string query = @"ESTADO_PRODUCTO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdProducto = IdProducto
            });
            return resultadoConsulta;
        }

        public async Task<Paginacion<ProductosResponse>> ListarProductosPaginado(int pageIndex, int pageSize)
        {
            string query = @"VER_PRODUCTOS_PAGINADO";
            var consulta = await _sqlConnection.QueryMultipleAsync(query, new
            {
                PageIndex= pageIndex,
                PageSize = pageSize
            });
            var productos = (await consulta.ReadAsync<ProductosResponse>()).ToList();
            var totalRegistros = await consulta.ReadSingleAsync<int>();
            var totalPages = (int)Math.Ceiling((double)totalRegistros / pageSize);

            var respuesta = new Paginacion<ProductosResponse>(productos, pageIndex, totalPages);
            return respuesta;

        }

        public async Task<IEnumerable<ProductosResponse>> Obtener()
        {
            string query = @"VER_PRODUCTOS";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductosResponse>(query);
            return resultadoConsulta;
        }

        public async Task<ProductosResponse> ObtenerPorId(Guid IdProducto)
        {
            string query = @"VER_PRODUCTO_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductosResponse>(query,
                new { IdProducto = IdProducto });
            return resultadoConsulta.FirstOrDefault();
        }


        private async Task VerificarProductoExiste(Guid IdProducto)
        {
            ProductosResponse? resutadoConsultaProducto = await ObtenerPorId(IdProducto);
            if (resutadoConsultaProducto == null)
                throw new Exception("no se encontro el producto");
        }
        public async Task<IEnumerable<ProductosResponse>> ObtenerProductosBuscados(string nombre)
        {
            string query = @"BUSCAR_PRODUCTOS";
            var resultadoConsulta = await _sqlConnection.QueryAsync<ProductosResponse>(query,
                new { NombreProducto = nombre });
            return resultadoConsulta;
        }

        public async Task<Paginacion<ProductosResponse>> ObtenerProductosXCategoria(Guid idCategoria, int pageIndex, int pageSize)
        {
            string query = @"ProductosXCategoria";
            var consulta = await _sqlConnection.QueryMultipleAsync(query, new
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                idCategoria = idCategoria
            });
            var productos = (await consulta.ReadAsync<ProductosResponse>()).ToList();
            var totalRegistros = await consulta.ReadSingleAsync<int>();
            var totalPages = (int)Math.Ceiling((double)totalRegistros / pageSize);

            var respuesta = new Paginacion<ProductosResponse>(productos, pageIndex, totalPages);
            return respuesta;
        }
    }
}
