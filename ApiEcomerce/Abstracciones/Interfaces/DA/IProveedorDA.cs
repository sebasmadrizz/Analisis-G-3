using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.DA
{
    public interface IProveedorDA
    {
        Task<IEnumerable<Proveedores>> Obtener();
        Task<Proveedores> ObtenerPorId(Guid IdProveedor);
        Task<Guid> Agregar(Proveedores proveedor);

		Task<Guid> Editar(Guid IdProveedor, Proveedores proveedor);

		Task<Guid> Eliminar(Guid IdProveedor);
	}
}
