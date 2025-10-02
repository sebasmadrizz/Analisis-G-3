using Abstracciones.Interfaces.Reglas;
using System.Text;

namespace Reglas
{
    public class ObtenerCorreoDesdeTokenRegla : IObtenerCorreoDesdeTokenRegla
    {
        

        public string ObtenerCorreoDesdeToken(string token)
        {

            // Normalizar padding del Base64
            int padding = 4 - (token.Length % 4);
            if (padding < 4) token = token.PadRight(token.Length + padding, '=');

            // Decodificar Base64
            var dataBytes = Convert.FromBase64String(token);
            string data = Encoding.UTF8.GetString(dataBytes);

            // El correo siempre está antes del primer '|'
            var partes = data.Split('|');
            return partes.Length > 0 ? partes[0] : null;
        }
    }
}
