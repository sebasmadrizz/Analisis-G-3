using Abstracciones.Interfaces.DA;
using Microsoft.Extensions.Configuration;

namespace DA.Repositorios
{
    public class RepositorioResetPassword : IRepositorioResetPassword
    {
        private readonly IConfiguration _configuration;
        public string _rutaWeb { get; }
        public RepositorioResetPassword(IConfiguration configuration)
        {
            _configuration = configuration;
            _rutaWeb = _configuration.GetSection("RutaWeb").Value;
            
        }
        public string ObtenerRutaWeb()
        {
            return $"{_rutaWeb}";
        }
    }
}
