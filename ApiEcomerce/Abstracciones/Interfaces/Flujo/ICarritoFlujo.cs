using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Abstracciones.Modelos.Carrito;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ICarritoFlujo
    {
        Task<CarritoResponse> ObtenerPorUsuario(Guid UsuarioId);
        Task<CarritoCorreo> ObtenerParaCorreo(Guid usuarioId);
        Task<CarritoResponse> ObtenerPorID(Guid CarritoId);

        Task<Guid> Agregar(CarritoBase carrito);

        Task<Guid> Editar(Guid CarritoId, CarritoBase carrito);

        Task<Guid> Eliminar(Guid CarritoId);
        Task<Guid> EliminarTotal(Guid CarritoId);

        Task<Guid> ActualizarTotal(Guid CarritoId);
        Task<int> EliminarCarritosExpirados(int minutosExpiracion);
    }
}
