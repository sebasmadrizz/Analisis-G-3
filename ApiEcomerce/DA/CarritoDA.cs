using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Entidades;
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;
using static Abstracciones.Modelos.Carrito;
using static Abstracciones.Modelos.Categorias;


namespace DA
{
    public class CarritoDA : ICarritoDA
    {
        private readonly IRepositorioDapper _repositorioDapper;
        private readonly SqlConnection _sqlConnection;
        private readonly ICarritoProductoDA _carritoProductoDA;

        public CarritoDA(IRepositorioDapper repositorioDapper, ICarritoProductoDA carritoProductoDA)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
            _carritoProductoDA = carritoProductoDA;
        }

        public async Task<Guid> ActualizarTotal(Guid CarritoId)
        {
            string query = @"TOTAL_CARRITO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                CarritoId = CarritoId
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Agregar(CarritoBase carrito)
        {
            string query = @"AGREGAR_CARRITO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {

                CarritoId = carrito.CarritoId,
                UsuarioId = carrito.UsuarioId,
                FechaCreacion = DateTime.Now,
                Total = carrito.Total,
            });
            return resultadoConsulta;
        }

        public async Task<Guid> Editar(Guid CarritoId, CarritoBase carrito)
        {
            await VerificarCarritoExiste(CarritoId);


            string query = @"EDITAR_CARRITO";

            var resultado = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                IdCarrito = CarritoId,
                Total = carrito.Total,
            });

            return resultado;
        }

        public async Task<Guid> Eliminar(Guid CarritoId)
        {
            await VerificarCarritoExiste(CarritoId);
            string query = @"ELIMINAR_CARRITO";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                CarritoId = CarritoId
            });
            return resultadoConsulta;
        }

        public async Task<int> EliminarCarritosExpirados(int minutosExpiracion)
        {
            string query = @"LIMPIEZA_CARRITOS";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                minutosExpiracion = minutosExpiracion
            });
            return resultadoConsulta;
        }

        public async Task<Guid> EliminarTotal(Guid CarritoId)
        {
            await VerificarCarritoExiste(CarritoId);
            string query = @"ELIMINAR_TOTAL";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<Guid>(query, new
            {
                CarritoId = CarritoId
            });
            return resultadoConsulta;
        }

        public async Task<CarritoCorreo> ObtenerParaCorreo(Guid usuarioId)
        {
            string query = "sObtenerDatosCarritoPorUsuario";

            var filas = await _sqlConnection.QueryAsync<dynamic>(
                query,
                new { UsuarioId = usuarioId },
                commandType: CommandType.StoredProcedure
            );

            var carrito = new CarritoCorreo();

            foreach (var fila in filas)
            {
                if (carrito.CarritoId == Guid.Empty)
                {
                    carrito.UsuarioId = fila.UsuarioId;
                    carrito.NombreUsuario = fila.NombreUsuario;
                    carrito.Apellido = fila.Apellido;
                    carrito.CorreoElectronico = fila.CorreoElectronico;
                    carrito.Telefono = fila.Telefono;
                    carrito.Direccion = fila.Direccion;

                    carrito.CarritoId = fila.CarritoId;
                    carrito.FechaCreacion = fila.FechaCreacion;
                    carrito.TotalCarrito = fila.TotalCarrito;
                }
                carrito.Productos.Add(new ProductoCarritoCorreo
                {
                    ProductosId = fila.ProductosId,
                    NombreProducto = fila.NombreProducto,
                    Marca = fila.Marca,
                    Precio = fila.Precio,
                    Cantidad = fila.Cantidad,
                    TotalLinea = fila.TotalLinea
                });
            }

            if (carrito.CarritoId == Guid.Empty)
                return null;

            return carrito;
        }
        public async Task<CarritoResponse> ObtenerPorID(Guid CarritoId)
        {
            string query = @"OBTENER_CARRITO_POR_ID";
            var resultadoConsulta = await _sqlConnection.QueryAsync<CarritoResponse>(
                query,
                new { IdCarrito = CarritoId }
            );

            var carrito = resultadoConsulta.FirstOrDefault();

            carrito.Productos = await _carritoProductoDA.ObtenerPorCarrito(carrito.CarritoId);

            return carrito;
        }

        public async Task<CarritoResponse> ObtenerPorUsuario(Guid UsuarioId)
        {
            string query = @"OBTENER_CARRITOS_POR_USUARIO";
            var resultadoConsulta = await _sqlConnection.QueryAsync<CarritoResponse>(
                query,
                new { UsuarioId = UsuarioId }
            );

            var carrito = resultadoConsulta.FirstOrDefault();

            if (carrito == null)
                return null;

            carrito.Productos = await _carritoProductoDA.ObtenerPorCarrito(carrito.CarritoId);

            return carrito;
        }

        private async Task VerificarCarritoExiste(Guid IdCarrito)
        {
            CarritoResponse? resutadoConsultaCarrito = await ObtenerPorID(IdCarrito);
            if (resutadoConsultaCarrito == null)
                throw new Exception("no se encontro el carrito");
        }
    }
}
