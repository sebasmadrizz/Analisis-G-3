using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;


namespace Flujo
{
    public class UsuarioFlujo: IUsuarioFlujo
    {
        private IUsuarioDA _usuarioDA;

        public UsuarioFlujo(IUsuarioDA usuarioDA)
        {
            _usuarioDA = usuarioDA;
        }

        public async Task<Guid?> CambiarContraseña(CambiarContraseña data)
        {
            return await _usuarioDA.CambiarContraseña(data);
        }

        public async Task<Guid?> CrearUsuario(Usuario usuario)
        {
            return await _usuarioDA.CrearUsuario(usuario);
        }

        public async Task<Guid?> CrearUsuarioEmpleado(Usuario usuario)
        {
            return await _usuarioDA.CrearUsuarioEmpleado(usuario);
        }

        public async Task<Guid> Desactivar(Guid idUsuario)
        {
            return await _usuarioDA.Desactivar(idUsuario);
        }

        public async Task<Usuario> DetalleUsuario(Guid idUsuario)
        {
            return await _usuarioDA.DetalleUsuario(idUsuario);
        }

        public async Task<Guid> EditarUsuario(Guid idUsuario, UsuarioEditar usuario)
        {
            return await _usuarioDA.EditarUsuario(idUsuario, usuario);
        }

        public async Task<Usuario> ObtenerUsuario(Usuario usuario)
        {
            return await _usuarioDA.ObtenerUsuario(usuario);
        }

    }
}
