using Abstracciones.Interfaces.DA;
using Microsoft.Extensions.Configuration;

namespace DA.Repositorios
{
    public class RepositorioSistemaArchivos : IRepositorioSistemaArchivos
    {
        private readonly IConfiguration _configuration;
        public string _ruta { get; }
        public string _rutaImagen { get; }
        public RepositorioSistemaArchivos(IConfiguration configuration)
        {
            _configuration = configuration;
            _ruta = _configuration.GetSection("RutaDocumentos").Value;
            _rutaImagen = _configuration.GetSection("RutaObtenerImagenes").Value;
        }
        public string ObtenerRuta()
        {
            return $"{_ruta}";
        }

        public string ObtenerRutaImagen()
        {
            return $"{_rutaImagen}";
        }
    }
}
