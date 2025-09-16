using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reglas
{
    public class DocumentoRegla : IDocumentoRegla
    {
        private IRepositorioSistemaArchivos _repositorioSistemaArchivos;
        private string _rutaArchivos;
        private string _rutaImagenes;
        private string nombreImagenVacia;
        public DocumentoRegla(IRepositorioSistemaArchivos repositorioSistemaArchivos)
        {
            _rutaArchivos = repositorioSistemaArchivos.ObtenerRuta();
            _rutaImagenes=repositorioSistemaArchivos.ObtenerRutaImagen();
            nombreImagenVacia = "productoSinImagen.png";
        }
        public async Task<string> GuardarDocumento(Documento imagen)
        {
            if(imagen==null)
                return $"{_rutaImagenes}{nombreImagenVacia}";
            var extension = Path.GetExtension(imagen.Nombre);
            File.WriteAllBytes($"{_rutaArchivos}\\{imagen.Id}{extension}", imagen.Contenido);
            return $"{_rutaImagenes}{imagen.Id}{extension}";
        }
        public async Task<string> GuardarDocumentoEditar(Documento imagen, string urlImagenAnterior)
        {
            if (imagen == null)
                return urlImagenAnterior;
            var extension = Path.GetExtension(imagen.Nombre);
            File.WriteAllBytes($"{_rutaArchivos}\\{imagen.Id}{extension}", imagen.Contenido);
            return $"{_rutaImagenes}{imagen.Id}{extension}";
        }

    }
}
