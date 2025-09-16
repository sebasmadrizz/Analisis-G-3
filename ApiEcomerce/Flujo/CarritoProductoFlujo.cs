using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using static Abstracciones.Modelos.CarritoProducto;

namespace Flujo
{
	public class CarritoProductoFlujo : ICarritoProductoFlujo
	{
        private readonly ICarritoProductoDA _carritoProductoDA;
        private readonly ICarritoProductoReglas _carritoProductoReglas;
		

		public CarritoProductoFlujo(ICarritoProductoDA carritoProductoDA, ICarritoProductoReglas carritoProductoReglas)
        {
            _carritoProductoDA = carritoProductoDA;
            _carritoProductoReglas = carritoProductoReglas;
        }


        public async Task<Guid> Agregar(Guid usuarioId, CarritoProductoRequest carritoProducto)
        {
            return await _carritoProductoReglas.Agregar(usuarioId, carritoProducto);
        }

        public async Task DescontarStock(Guid ProductoId, int cantidadSolicitada)
        {
            await _carritoProductoDA.DescontarStock(ProductoId, cantidadSolicitada);
        }

        public async Task DevolverStock(Guid ProductoId, int cantidad)
        {
            await _carritoProductoDA.DevolverStock(ProductoId, cantidad);
        }

        public async Task<Guid> Editar(Guid CarritoProductoId, CarritoProductoRequest carritoproducto)
		{
            return await _carritoProductoReglas.Editar(CarritoProductoId, carritoproducto);
		}




		public async Task<Guid> Eliminar(Guid CarritoProductoId)
		{
            return await _carritoProductoReglas.Eliminar(CarritoProductoId);
        }

		public async Task<List<CarritoProductoResponse>> ObtenerPorCarrito(Guid CarritoId)
		{
			return await _carritoProductoDA.ObtenerPorCarrito(CarritoId);
		}


		public async Task<CarritoProductoResponse> ObtenerPorID(Guid CarritoProductoId)
		{
			return await _carritoProductoDA.ObtenerPorID(CarritoProductoId);
		}

		public async Task<bool> ValidarStock( Guid ProductoId, int cantidadSolicitada)
		{
		
			// Validar stock usando las reglas de negocio
			bool hayStock = await _carritoProductoReglas.ValidarStock( ProductoId, cantidadSolicitada);
			if (!hayStock)
			{
				throw new Exception("No hay stock suficiente para realizar la venta");
			}

			// Si hay stock, proceder con la venta
			var resultado = await _carritoProductoDA.ValidarStock( ProductoId, cantidadSolicitada);
			return resultado;
		}
	}
}
