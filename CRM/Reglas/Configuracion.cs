using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private IConfiguration _configuration;
        public Configuracion(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string ObtenerMetodo(string seccion, string nombre)
        {
            string? UrlBase = ObtenerUrlBase(seccion);
            var metodo = _configuration.GetSection(seccion).Get<APIEndPoint>
                ().Metodos.Where(m => m.Nombre == nombre).FirstOrDefault().Valor;
            return $"{UrlBase}/{metodo}";
        }

        private string? ObtenerUrlBase(string seccion)
        {
            return _configuration.GetSection(seccion).Get<APIEndPoint>().UrlBase;
        }

        public string ObtenerValor(string llave)
        {
            return _configuration.GetSection(llave).Value;
        }
    }
}

