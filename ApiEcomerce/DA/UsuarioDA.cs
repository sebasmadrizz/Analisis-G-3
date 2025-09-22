
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Helpers;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class UsuarioDA: IUsuarioDA
    {
        IRepositorioDapper _repositorioDapper;
        private SqlConnection _SqlConnection;

        public UsuarioDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _SqlConnection = repositorioDapper.ObtenerRepositorio();
        }

        public async Task<Guid?> CrearUsuario(Usuario usuario)
        {
            if (await ExisteCorreo(usuario.CorreoElectronico))
                return null;


            var sql = @"[AgregarUsuario]";
            var resultado = await _SqlConnection.ExecuteScalarAsync<Guid>(sql, new { NombreUsuario = usuario.NombreUsuario, PasswordHash = usuario.PasswordHash, CorreoElectronico = usuario.CorreoElectronico, IdEstado = 1 , Telefono = usuario.Telefono, Direccion = usuario.Direccion,Apellido=usuario.Apellido});
            return resultado;
        }

        public async Task<Usuario> DetalleUsuario(Guid idUsuario)
        {
            var sql = @"[DetalleUsuario]";
            var consulta = await _SqlConnection.QueryAsync<Abstracciones.Modelos.Usuario>(sql, new { IdUsuario = idUsuario });
            return consulta.FirstOrDefault();

        }

        public async Task<Guid> EditarUsuario(Guid idUsuario, UsuarioEditar usuario)
        {
            var sql = @"[EditarUsuario]";
            var resultado = await _SqlConnection.ExecuteScalarAsync<Guid>(sql, new { IdUsuario = idUsuario, NombreUsuario = usuario.NombreUsuario, CorreoElectronico = usuario.CorreoElectronico, Telefono = usuario.Telefono, Direccion = usuario.Direccion, Apellido = usuario.Apellido });
            return resultado;
        }

        public async Task<IEnumerable<Perfiles>> ObtenerPerfilesUsuario(Usuario usuario)
        {
            string sql = @"[ObtenerPerfilesxUsuario]";
            var consulta = await _SqlConnection.QueryAsync<Abstracciones.Entidades.Perfiles>(sql, new { CorreoElectronico = usuario.CorreoElectronico, NombreUsuario = usuario.NombreUsuario });
            return Convertidor.ConvertirLista<Abstracciones.Entidades.Perfiles, Abstracciones.Modelos.Perfiles>(consulta);
        }

        public async Task<Usuario> ObtenerUsuario(Usuario usuario)
        {
            string sql = @"[ObtenerUsuario]";
            var consulta = await _SqlConnection.QueryAsync<Abstracciones.Entidades.Usuario>(sql, new
            {
                CorreoElectronico = usuario.CorreoElectronico
                ,
                NombreUsuario = usuario.NombreUsuario
            });
            return Convertidor.Convertir<Abstracciones.Entidades.Usuario, Abstracciones.Modelos.Usuario>(consulta.FirstOrDefault());
        }
        public async Task<bool> ExisteCorreo(string correo) 
        { 
            
            string sql = @"[ExisteCorreo]";
                var resultado = await _SqlConnection.ExecuteScalarAsync<int>(sql, new { CorreoElectronico = correo });
                if (resultado == 1) 
                {
                    return true;
                }
            return false;

        }

        public async Task<Guid?> CrearUsuarioEmpleado(Usuario usuario)
        {
            if (await ExisteCorreo(usuario.CorreoElectronico))
                return null;


            var sql = @"[AgregarUsuarioEmpleado]";
            var resultado = await _SqlConnection.ExecuteScalarAsync<Guid>(sql, new { NombreUsuario = usuario.NombreUsuario, PasswordHash = usuario.PasswordHash, CorreoElectronico = usuario.CorreoElectronico, IdEstado = 1, Telefono = usuario.Telefono, Direccion = usuario.Direccion, Apellido = usuario.Apellido });
            return resultado;
        }

        public async Task<Guid?> CambiarContraseña(CambiarContraseña data)
        {
            var sql = @"[CambiarContrasena]";
            var resultado = await _SqlConnection.ExecuteScalarAsync<Guid>(sql, new {  NuevaContraseña=data.NuevaContraseña, Correo=data.Correo });
            return resultado;
        }
    }
}
