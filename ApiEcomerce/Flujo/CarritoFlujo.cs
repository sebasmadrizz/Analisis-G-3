using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;
using static Abstracciones.Modelos.Carrito;

namespace Flujo
{
    public class CarritoFlujo : ICarritoFlujo
    {
        private readonly ICarritoDA _carritoDA;

        public CarritoFlujo(ICarritoDA carritoDA)
        {
            _carritoDA = carritoDA;
        }

        public async Task<Guid> Agregar(CarritoBase carrito)
        {
            return await _carritoDA.Agregar(carrito);
        }

        public async Task<Guid> Editar(Guid CarritoId, CarritoBase carrito)
        {
            return await _carritoDA.Editar(CarritoId,carrito);
        }

        public async Task<Guid> Eliminar(Guid CarritoId)
        {
            return await _carritoDA.Eliminar(CarritoId);
        }

        public async Task<CarritoResponse> ObtenerPorUsuario(Guid UsuarioId)
        {
            return await _carritoDA.ObtenerPorUsuario(UsuarioId);
        }

        public async Task<CarritoResponse> ObtenerPorID(Guid CarritoId)
        {
            return await _carritoDA.ObtenerPorID(CarritoId);
        }

        public async Task<Guid> ActualizarTotal(Guid CarritoId)
        {
            return await _carritoDA.ActualizarTotal(CarritoId);
        }

        public async Task<Guid> EliminarTotal(Guid CarritoId)
        {
            return await _carritoDA.EliminarTotal(CarritoId);
        }

        public async Task<int> EliminarCarritosExpirados(int minutosExpiracion)
        {
            return await _carritoDA.EliminarCarritosExpirados(minutosExpiracion);
        }

        public async  Task<CarritoCorreo> ObtenerParaCorreo(Guid usuarioId)
        {
            return await _carritoDA.ObtenerParaCorreo(usuarioId);
        }
    }
}
