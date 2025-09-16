using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ProveedorFlujo: IProveedorFlujo
    {
        private readonly IProveedorDA _proveedorDA;
        public ProveedorFlujo(IProveedorDA proveedorDA)
        {
            _proveedorDA = proveedorDA;
        }
        public async Task<IEnumerable<Proveedores>> Obtener()
        {
            return await _proveedorDA.Obtener();
        }
        public async Task<Proveedores> ObtenerPorId(Guid IdProveedor)
        {
            return await _proveedorDA.ObtenerPorId(IdProveedor);
        }
        public async Task<Guid> Agregar(Proveedores proveedor)
        {
            return await _proveedorDA.Agregar(proveedor);
        }

		public async Task<Guid> Editar(Guid IdProveedor, Proveedores proveedor)
		{
			return await _proveedorDA.Editar(IdProveedor, proveedor);
		}

		public async Task<Guid> Eliminar(Guid IdProveedor)
		{
            return await _proveedorDA.Eliminar(IdProveedor);
		}
	}
}
