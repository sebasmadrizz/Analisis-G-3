using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IPuestosFlujo
    {
        Task<IEnumerable<PuestosResponse>> Obtener();
        Task<PuestosResponse> ObtenerPorId(Guid PuestosId);
        Task<Guid> Agregar(Puestos puestos);

        Task<Guid> Editar(Guid PuestosId, Puestos puestos);

        Task<Guid> Eliminar(Guid PuestosId);
    }
}
