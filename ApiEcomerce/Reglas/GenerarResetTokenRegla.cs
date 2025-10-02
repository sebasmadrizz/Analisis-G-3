using Abstracciones.Interfaces.Reglas;
using System.Security.Cryptography;
using System.Text;
namespace Reglas
{
    public class GenerarResetTokenRegla : IGenerarResetTokenRegla
    {
        public string GenerarResetToken(string correo)
        {
            
            var bytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            string aleatorio = Convert.ToBase64String(bytes)
                                .Replace("+", "-")
                                .Replace("/", "_")
                                .Replace("=", "");

           
            string data = $"{correo}|{aleatorio}";

            
            var dataBytes = Encoding.UTF8.GetBytes(data);

            
            string token = Convert.ToBase64String(dataBytes)
                            .Replace("+", "-")
                            .Replace("/", "_")
                            .Replace("=", "");

            return token;
        }
    }
}
