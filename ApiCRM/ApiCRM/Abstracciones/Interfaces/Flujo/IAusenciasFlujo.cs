using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IAusenciasFlujo
    {
        Task<IEnumerable<AusenciasResponse>> Obtener();
        Task<AusenciasResponse> ObtenerPorId(Guid AusenciasId);
        Task<Guid> Agregar(Ausencias ausencias);

        Task<Guid> Editar(Guid AusenciasId, Ausencias ausencias);

        Task<Guid> Eliminar(Guid AusenciasId);
    }
}
