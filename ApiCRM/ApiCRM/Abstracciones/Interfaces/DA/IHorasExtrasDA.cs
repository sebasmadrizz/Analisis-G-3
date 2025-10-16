using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IHorasExtrasDA
    {
        Task<IEnumerable<HorasExtrasResponse>> Obtener();
        Task<HorasExtrasResponse> ObtenerPorId(Guid HorasExtrasId);
        Task<Guid> Agregar(HorasExtras horasExtras);

        Task<Guid> Editar(Guid HorasExtrasId, HorasExtras horasExtras);

        Task<Guid> Eliminar(Guid HorasExtrasId);
    }
}
