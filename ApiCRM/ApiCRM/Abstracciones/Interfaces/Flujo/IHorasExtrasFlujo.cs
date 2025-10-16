using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IHorasExtrasFlujo
    {
        Task<IEnumerable<HorasExtrasResponse>> Obtener();
        Task<HorasExtrasResponse> ObtenerPorId(Guid HorasExtrasId);
        Task<Guid> Agregar(HorasExtras horasExtras);

        Task<Guid> Editar(Guid HorasExtrasId, HorasExtras horasExtras);

        Task<Guid> Eliminar(Guid HorasExtrasId);
    }
}
