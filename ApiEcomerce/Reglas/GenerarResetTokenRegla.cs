using Abstracciones.Interfaces.Reglas;
using System.Security.Cryptography;
namespace Reglas
{
    public class GenerarResetTokenRegla : IGenerarResetTokenRegla
    {
        public string GenerarResetToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes)
       .Replace("+", "-")
       .Replace("/", "_")
       .Replace("=", "");

        }
    }
}
