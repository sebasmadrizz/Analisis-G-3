using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.Carrito;
using static Abstracciones.Modelos.CarritoProducto;

namespace Abstracciones.Interfaces.API
{
    public interface ICarritoController
    {

        Task<IActionResult> ObtenerPorUsuario(Guid usuarioId);
        Task<IActionResult> ObtenerParaCorreo();
        Task<IActionResult> ObtenerPorID(Guid CarritoId);


        Task<IActionResult> Agregar(CarritoBase carrito);

        Task<IActionResult> Editar(Guid CarritoId, CarritoBase carrito);

        Task<IActionResult> Eliminar(Guid CarritoId);

        Task<IActionResult> EliminarTotal(Guid CarritoId);

        Task<IActionResult> ActualizarTotal(Guid CarritoId);
    }
}
