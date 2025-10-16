using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class PuestosFlujo: IPuestosFlujo
    {
        private readonly IPuestosDA _puestosDA;
        public PuestosFlujo(IPuestosDA puestosDA)
        {
            _puestosDA = puestosDA;
        }

        public async Task<Guid> Agregar(Puestos puestos)
        {
            return await _puestosDA.Agregar(puestos);
        }

        public async Task<Guid> Editar(Guid PuestosId, Puestos puestos)
        {
            return await _puestosDA.Editar(PuestosId,puestos);
        }

        public async Task<Guid> Eliminar(Guid PuestosId)
        {
            return await _puestosDA.Eliminar(PuestosId);
        }

        public async Task<IEnumerable<PuestosResponse>> Obtener()
        {
            return await _puestosDA.Obtener();
        }

        public async Task<PuestosResponse> ObtenerPorId(Guid PuestosId)
        {
            return await _puestosDA.ObtenerPorId(PuestosId);
        }
    }
}
