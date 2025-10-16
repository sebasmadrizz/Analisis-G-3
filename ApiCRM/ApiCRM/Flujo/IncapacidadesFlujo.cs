using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class IncapacidadesFlujo: IIncapacidadesFlujo
    {
        private readonly IIncapacidadesDA _incapacidadesDA;
        public IncapacidadesFlujo(IIncapacidadesDA incapacidadesDA)
        {
            _incapacidadesDA = incapacidadesDA;
        }

        public async Task<Guid> Agregar(Incapacidades incapacidad)
        {
            return await _incapacidadesDA.Agregar(incapacidad);
        }

        public async Task<Guid> Editar(Guid IncapacidadesId, Incapacidades incapacidad)
        {
            return await _incapacidadesDA.Editar(IncapacidadesId, incapacidad);
        }

        public async Task<Guid> Eliminar(Guid IncapacidadesId)
        {
            return await _incapacidadesDA.Eliminar(IncapacidadesId);
        }

        public async Task<IEnumerable<IncapacidadesResponse>> Obtener()
        {
            return await _incapacidadesDA.Obtener();
        }

        public async Task<IncapacidadesResponse> ObtenerPorId(Guid IncapacidadesId)
        {
            return await _incapacidadesDA.ObtenerPorId(IncapacidadesId);
        }
    }
}
