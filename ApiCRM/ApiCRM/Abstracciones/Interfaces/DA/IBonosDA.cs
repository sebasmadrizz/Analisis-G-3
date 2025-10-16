using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IBonosDA
    {
        Task<IEnumerable<BonosResponse>> Obtener();
        Task<BonosResponse> ObtenerPorId(Guid BonosId);
        Task<Guid> Agregar(Bonos bonos);

        Task<Guid> Editar(Guid BonosId, Bonos bonos);

        Task<Guid> Eliminar(Guid BonosId);
    }
}
