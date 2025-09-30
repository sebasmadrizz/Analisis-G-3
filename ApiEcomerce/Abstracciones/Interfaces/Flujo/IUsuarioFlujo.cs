using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IUsuarioFlujo
    {
        Task<Guid?> CrearUsuario(Usuario usuario);
        Task<Guid?> CrearUsuarioEmpleado(Usuario usuario);
        Task<Guid?> CambiarContraseña(CambiarContraseña data);
        Task<Usuario> ObtenerUsuario(Usuario usuario);
        Task<Guid> EditarUsuario(Guid idUsuario, UsuarioEditar usuario);
        Task<Usuario> DetalleUsuario(Guid idUsuario);

        Task<Guid> Desactivar(Guid idUsuario);
    }
}
