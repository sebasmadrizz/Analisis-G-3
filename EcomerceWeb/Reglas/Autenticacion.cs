using Abstracciones.Modelos.Seguridad;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public static class Autenticacion
    {
        public static byte[] GenerarHash(string contrasenia)
        {
            using (SHA256 shaHash = SHA256.Create())
            {
                byte[] bytes = shaHash.ComputeHash(Encoding.UTF8.GetBytes(contrasenia));
                return bytes;
            }
        }
        public static string ObtenerHash(byte[] hash)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
        public static JwtSecurityToken? leerToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            return jsonToken;
        }

        public static List<Claim> GenerarClaims(JwtSecurityToken? jwtToken, string accessToken )
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("usuario", jwtToken.Claims.First(c => c.Type == "usuario").Value));
            claims.Add(new Claim(ClaimTypes.Name, jwtToken.Claims.First(c => c.Type == "usuario").Value));
            claims.Add(new Claim(ClaimTypes.Role, jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value));

            claims.Add(new Claim(ClaimTypes.Email, jwtToken.Claims.First(c => c.Type == "correoElectronico").Value));
            claims.Add(new Claim("idUsuario", jwtToken.Claims.First(c => c.Type == "idUsuario").Value));
            claims.Add(new Claim("Token", accessToken));
            
            return claims;
        }
    }
}
