using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IBonosFlujo
    {
        Task<IEnumerable<BonosResponse>> Obtener();
        Task<BonosResponse> ObtenerPorId(Guid BonosId);
        Task<Guid> Agregar(Bonos bonos);

        Task<Guid> Editar(Guid BonosId, Bonos bonos);

        Task<Guid> Eliminar(Guid BonosId);
    }
}
