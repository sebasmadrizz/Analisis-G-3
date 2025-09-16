using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IDocumentoRegla
    {
        Task<string> GuardarDocumento(Documento imagen);
        Task<string> GuardarDocumentoEditar(Documento imagen,string urlImagenAnterior);

    }
}
