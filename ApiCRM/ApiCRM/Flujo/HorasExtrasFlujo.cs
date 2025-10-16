using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class HorasExtrasFlujo: IHorasExtrasFlujo
    {
        private readonly IHorasExtrasDA _horasExtrasDA;
        public HorasExtrasFlujo(IHorasExtrasDA horasExtrasDA)
        {
            _horasExtrasDA = horasExtrasDA;
        }

        public async Task<Guid> Agregar(HorasExtras horasExtras)
        {
            return await _horasExtrasDA.Agregar(horasExtras);
        }

        public async Task<Guid> Editar(Guid HorasExtrasId, HorasExtras horasExtras)
        {
            return await _horasExtrasDA.Editar(HorasExtrasId,horasExtras);
        }

        public async Task<Guid> Eliminar(Guid HorasExtrasId)
        {
            return await _horasExtrasDA.Eliminar(HorasExtrasId);
        }

        public async Task<IEnumerable<HorasExtrasResponse>> Obtener()
        {
            return await _horasExtrasDA.Obtener();
        }

        public async Task<HorasExtrasResponse> ObtenerPorId(Guid HorasExtrasId)
        {
            return await _horasExtrasDA.ObtenerPorId(HorasExtrasId);
        }
    }
}
