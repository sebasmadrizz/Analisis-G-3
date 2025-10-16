using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class HorarioFlujo: IHorarioFlujo
    {
        private readonly IHorarioDA _horarioDA;
        public HorarioFlujo(IHorarioDA horarioDA)
        {
            _horarioDA = horarioDA;
        }

        public async Task<Guid> Agregar(Horario horario)
        {
            return await _horarioDA.Agregar(horario);
        }

        public async Task<Guid> Editar(Guid HorarioId, Horario horario)
        {
            return await _horarioDA.Editar(HorarioId,horario);
        }

        public async Task<Guid> Eliminar(Guid HorarioId)
        {
            return await _horarioDA.Eliminar(HorarioId);
        }

        public async Task<IEnumerable<HorarioResponse>> Obtener()
        {
            return await _horarioDA.Obtener();
        }

        public async Task<HorarioResponse> ObtenerPorId(Guid HorarioId)
        {
            return await _horarioDA.ObtenerPorId(HorarioId);
        }
    }
}
