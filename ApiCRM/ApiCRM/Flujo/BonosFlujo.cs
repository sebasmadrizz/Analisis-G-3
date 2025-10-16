using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class BonosFlujo: IBonosFlujo
    {
        private readonly IBonosDA _bonosDA;
        public BonosFlujo(IBonosDA bonosDA)
        {
            _bonosDA = bonosDA;
        }

        public async Task<Guid> Agregar(Bonos bonos)
        {
            return await _bonosDA.Agregar(bonos);
        }

        public async Task<Guid> Editar(Guid BonosId, Bonos bonos)
        {
            return await _bonosDA.Editar(BonosId,bonos);
        }

        public async Task<Guid> Eliminar(Guid BonosId)
        {
            return await _bonosDA.Eliminar(BonosId);
        }

        public async Task<IEnumerable<BonosResponse>> Obtener()
        {
            return await _bonosDA.Obtener();
        }

        public async Task<BonosResponse> ObtenerPorId(Guid BonosId)
        {
            return await _bonosDA.ObtenerPorId(BonosId);
        }
    }
}
