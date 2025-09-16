
using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IUsuarioDA
    {
        Task<Usuario> ObtenerUsuario(Usuario usuario);
        Task<IEnumerable<Perfiles>> ObtenerPerfilesUsuario(Usuario usuario);
        Task<Guid?> CrearUsuario(Usuario usuario);
        Task<Guid?> CrearUsuarioEmpleado(Usuario usuario);
        Task<Guid> EditarUsuario(Guid idUsuario, UsuarioEditar usuario);
        Task<Usuario> DetalleUsuario(Guid idUsuario);
    }
}
