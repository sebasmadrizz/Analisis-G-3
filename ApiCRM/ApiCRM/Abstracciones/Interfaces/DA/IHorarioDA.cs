using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IHorarioDA
    {
        Task<IEnumerable<HorarioResponse>> Obtener();
        Task<HorarioResponse> ObtenerPorId(Guid HorarioId);
        Task<Guid> Agregar(Horario horario);

        Task<Guid> Editar(Guid HorarioId, Horario horario);

        Task<Guid> Eliminar(Guid HorarioId);
    }
}
