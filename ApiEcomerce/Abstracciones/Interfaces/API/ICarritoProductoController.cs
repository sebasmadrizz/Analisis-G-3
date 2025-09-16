using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Abstracciones.Modelos.CarritoProducto;

namespace Abstracciones.Interfaces.API
{
	public interface ICarritoProductoController
	{


		Task<IActionResult> ObtenerPorCarrito(Guid CarritoId);
		Task<IActionResult> ObtenerPorID(Guid CarritoProductoId);


		Task<IActionResult> Agregar(CarritoProductoRequest carritoProducto);

		Task<IActionResult> Editar(Guid CarritoProductoId, CarritoProductoRequest carritoProducto);

		Task<IActionResult> Eliminar(Guid CarritoProductoId);

		Task<IActionResult> ValidarStock( Guid ProductoId, int cantidadSolicitada);

	}
}
