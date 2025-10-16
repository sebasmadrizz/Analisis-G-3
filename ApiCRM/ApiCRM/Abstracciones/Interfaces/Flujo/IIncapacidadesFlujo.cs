using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IIncapacidadesFlujo
    {
        Task<IEnumerable<IncapacidadesResponse>> Obtener();
        Task<IncapacidadesResponse> ObtenerPorId(Guid IncapacidadesId);
        Task<Guid> Agregar(Incapacidades incapacidad);

        Task<Guid> Editar(Guid IncapacidadesId, Incapacidades incapacidad);

        Task<Guid> Eliminar(Guid IncapacidadesId);
    }
}
