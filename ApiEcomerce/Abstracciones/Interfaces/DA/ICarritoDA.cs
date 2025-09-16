using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.Modelos;
using static Abstracciones.Modelos.Carrito;

namespace Abstracciones.Interfaces.DA
{
    public interface ICarritoDA
    {


        Task<CarritoResponse> ObtenerPorUsuario(Guid usuarioId);//para ver si crear o no
        Task<CarritoCorreo> ObtenerParaCorreo(Guid usuarioId);
        Task<CarritoResponse> ObtenerPorID(Guid CarritoId);// para los demas metodos



        Task<Guid> Agregar(CarritoBase carrito);//se agrega solo cuando el cliente no tenga, se hace cuando el cliente agregue el producto internasmenter

        Task<Guid> Editar(Guid CarritoId, CarritoBase carrito);//edita el total

        Task<Guid> Eliminar(Guid CarritoId);//cuando se elimina se deberia eliminar sus productos tambien

        Task<Guid> EliminarTotal(Guid CarritoId);

        Task<Guid> ActualizarTotal(Guid CarritoId);
        Task<int> EliminarCarritosExpirados(int minutosExpiracion);
    }
}
