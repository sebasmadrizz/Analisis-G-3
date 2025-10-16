using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IHorarioFlujo
    {
        Task<IEnumerable<HorarioResponse>> Obtener();
        Task<HorarioResponse> ObtenerPorId(Guid HorarioId);
        Task<Guid> Agregar(Horario horario);

        Task<Guid> Editar(Guid HorarioId, Horario horario);

        Task<Guid> Eliminar(Guid HorarioId);
    }
}
