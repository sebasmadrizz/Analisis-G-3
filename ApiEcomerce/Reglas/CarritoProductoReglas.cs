using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using static Abstracciones.Modelos.Carrito;
using static Abstracciones.Modelos.CarritoProducto;

namespace Reglas
{
    public class CarritoProductoReglas : ICarritoProductoReglas
    {
        private readonly ICarritoProductoDA _carritoProductoDA;
        private readonly ICarritoDA _carritoDA;

        public CarritoProductoReglas(ICarritoProductoDA carritoProductoDA, ICarritoDA carritoDA)
        {
            _carritoProductoDA = carritoProductoDA;
            _carritoDA = carritoDA;
        }


        public async Task<Guid> Agregar(Guid usuarioId, CarritoProductoRequest carritoProducto)
        {
            var carritoResponse = await _carritoDA.ObtenerPorUsuario(usuarioId);

            if (carritoResponse == null)
            {
                var nuevoCarrito = new CarritoBase
                {
                    CarritoId = Guid.NewGuid(),
                    UsuarioId = usuarioId,
                    FechaCreacion = DateTime.UtcNow,
                    Total = 0
                };

                await _carritoDA.Agregar(nuevoCarrito);

                carritoResponse = new CarritoResponse
                {
                    CarritoId = nuevoCarrito.CarritoId,
                    Total = nuevoCarrito.Total,
                    FechaCreacion = nuevoCarrito.FechaCreacion,
                    Productos = new List<CarritoProductoResponse>()
                };
            }

            var productoExistente = carritoResponse.Productos
                .FirstOrDefault(p => p.ProductosId == carritoProducto.ProductosId);

            if (productoExistente != null)
            {
                int cantidadExtra = carritoProducto.Cantidad;
                var stockValido = await _carritoProductoDA.ValidarStock(carritoProducto.ProductosId, cantidadExtra);

                if (!stockValido)
                    throw new Exception("No hay stock suficiente para realizar la venta");
                await _carritoProductoDA.DescontarStock(carritoProducto.ProductosId, cantidadExtra);
                productoExistente.Cantidad += cantidadExtra;
                await _carritoProductoDA.Editar(productoExistente.CarritoProductoId, new CarritoProductoRequest
                {
                    CarritoId = productoExistente.CarritoId,
                    ProductosId = productoExistente.ProductosId,
                    Cantidad = productoExistente.Cantidad
                });
                await _carritoDA.ActualizarTotal(productoExistente.CarritoId);


                return productoExistente.CarritoProductoId;
            }
            else
            {
                var stockValido = await _carritoProductoDA.ValidarStock(carritoProducto.ProductosId, carritoProducto.Cantidad);

                if (!stockValido)
                    throw new Exception("No hay stock suficiente para realizar la venta");

                await _carritoProductoDA.DescontarStock(carritoProducto.ProductosId, carritoProducto.Cantidad);
                var resultado = await _carritoProductoDA.Agregar(new CarritoProductoRequest
                {
                    CarritoId = carritoResponse.CarritoId,
                    ProductosId = carritoProducto.ProductosId,
                    Cantidad = carritoProducto.Cantidad
                });

                await _carritoDA.ActualizarTotal(carritoResponse.CarritoId);

                return resultado;
            }
        }


        public async Task<Guid> Editar(Guid carritoProductoId, CarritoProductoRequest carritoProducto)
        {
           

            var productoActual = await _carritoProductoDA.ObtenerPorID(carritoProductoId);

            int diferencia = carritoProducto.Cantidad - productoActual.Cantidad;

            if (diferencia > 0)
            {
                var stockValido = await _carritoProductoDA.ValidarStock(carritoProducto.ProductosId, diferencia);
                if (!stockValido)
                    throw new Exception("No hay stock suficiente para realizar la venta");
                await _carritoProductoDA.DescontarStock(carritoProducto.ProductosId, diferencia);
            }
            else if (diferencia < 0)
            {
                await _carritoProductoDA.DevolverStock(carritoProducto.ProductosId, -diferencia);
            }

            var carritoId = await _carritoProductoDA.Editar(carritoProductoId, carritoProducto);
            await _carritoDA.ActualizarTotal(carritoId);

            return carritoId;
        }


        public async Task<Guid> Eliminar(Guid carritoProductoId)
        {

            var carritoProducto = await _carritoProductoDA.ObtenerPorID(carritoProductoId);
            await _carritoProductoDA.DevolverStock(carritoProducto.ProductosId, carritoProducto.Cantidad);
            var carritoId = await _carritoProductoDA.Eliminar(carritoProductoId);
            await _carritoDA.ActualizarTotal(carritoId);

            return carritoId;
        }




        //estas lineas no sirven
        public async Task<bool> ValidarStock( Guid productoId, int cantidadSolicitada)
		{
			var stockDisponible = await _carritoProductoDA.ValidarStock( productoId, cantidadSolicitada);
			return stockDisponible;
		}

	}
}
