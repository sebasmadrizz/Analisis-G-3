using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Reglas
{
    public class AutenticacionReglas : IAutenticacionReglas
    {
        public IConfiguration _configuration;
        public IUsuarioDA _usuarioDA;

        public AutenticacionReglas(IConfiguration configuration, IUsuarioDA usuarioDA)
        {
            _configuration = configuration;
            _usuarioDA = usuarioDA;
        }

        public async Task<Token> LoginAsync(Login login)
        {
            Token respuestaToken = new Token() { AccessToken = string.Empty, ValidacionExitosa = false };
            var resultadoVerficacionCredenciales = await VerficarLoginAsync(login);
            if (!resultadoVerficacionCredenciales)
                return respuestaToken;
            TokenConfiguracion tokenConfiguracion = _configuration.GetSection("Token").Get<TokenConfiguracion>();
            JwtSecurityToken token = await GenerarTokenJWT(login, tokenConfiguracion);
            respuestaToken.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
            respuestaToken.ValidacionExitosa = true;
            return respuestaToken;
        }

        private async Task<bool> VerficarLoginAsync(Login login)
        {
            var usuario = await _usuarioDA.ObtenerUsuario(new Usuario { NombreUsuario = login.NombreUsuario, CorreoElectronico = login.CorreoElectronico });
            if (usuario == null)
                return false;
            return (login != null && login.PasswordHash == usuario.PasswordHash);

        }

        private async Task<JwtSecurityToken> GenerarTokenJWT(Login login, TokenConfiguracion tokenConfiguracion)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfiguracion.Key));
            List<Claim> claims = await GenerarClaims(login);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                tokenConfiguracion.Issuer,
                tokenConfiguracion.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(tokenConfiguracion.Expires),
                signingCredentials: credentials
            );
            return token;

        }

        private async Task<List<Claim>> GenerarClaims(Login login)
        {
            var usuario = await ObtenerUsuario(login);
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("usuario", login.NombreUsuario));
            claims.Add(new Claim("servicio", login.IdServicio.ToString()));
            claims.Add(new Claim("idUsuario", usuario.Id.ToString()));
            claims.Add(new Claim("correoElectronico", usuario.CorreoElectronico.ToString()));

            var perfiles = await ObtenerPerfiles(login);
            foreach (var perfil in perfiles)
            {
                claims.Add(new Claim(ClaimTypes.Role, perfil.Id.ToString()));
            }
            return claims;
        }

        private async Task<IEnumerable<Perfiles>> ObtenerPerfiles(Login login)
        {
            return await _usuarioDA.ObtenerPerfilesUsuario(new Usuario { NombreUsuario = login.NombreUsuario, CorreoElectronico = login.CorreoElectronico });
        }
        private async Task<Usuario> ObtenerUsuario(Login login)
        {
            return await _usuarioDA.ObtenerUsuario(new Usuario { NombreUsuario = login.NombreUsuario, CorreoElectronico = login.CorreoElectronico });
        }

    }
}
