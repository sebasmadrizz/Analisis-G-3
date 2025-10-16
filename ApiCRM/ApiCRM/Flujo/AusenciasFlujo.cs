using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class AusenciasFlujo: IAusenciasFlujo
    {
        private readonly IAusenciasDA _ausenciasDA;
        public AusenciasFlujo(IAusenciasDA ausenciasDA)
        {
            _ausenciasDA = ausenciasDA;
        }

        public async Task<Guid> Agregar(Ausencias ausencias)
        {
            return await _ausenciasDA.Agregar(ausencias);
        }

        public async Task<Guid> Editar(Guid AusenciasId, Ausencias ausencias)
        {
            return await _ausenciasDA.Editar(AusenciasId,ausencias);
        }

        public async Task<Guid> Eliminar(Guid AusenciasId)
        {
            return await _ausenciasDA.Eliminar(AusenciasId);
        }

        public async Task<IEnumerable<AusenciasResponse>> Obtener()
        {
            return await _ausenciasDA.Obtener();
        }

        public async Task<AusenciasResponse> ObtenerPorId(Guid AusenciasId)
        {
            return await _ausenciasDA.ObtenerPorId(AusenciasId);
        }
    }
}
