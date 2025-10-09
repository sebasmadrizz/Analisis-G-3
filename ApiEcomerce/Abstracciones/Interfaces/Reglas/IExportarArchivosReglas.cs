using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.Reglas
{
    public interface IExportarArchivosReglas
    {
        byte[] ExportExel<T>(IEnumerable<T> data);
        byte[] ExportPdf<T>(IEnumerable<T> data);

    }
}   
