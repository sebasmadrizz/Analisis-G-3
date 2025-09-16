using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Abstracciones.Modelos.CarritoProducto;

namespace Abstracciones.Interfaces.Reglas
{
    public interface ICarritoProductoReglas
    {
        Task<Guid> Agregar(Guid usuarioId, CarritoProductoRequest carritoProducto);

        Task<Guid> Eliminar(Guid carritoProductoId);

        Task<Guid> Editar(Guid carritoProductoId, CarritoProductoRequest carritoProducto);
		Task<bool> ValidarStock( Guid ProductoId, int cantidadSolicitada);
	}
}
