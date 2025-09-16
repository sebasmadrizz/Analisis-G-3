using Abstracciones.Interfaces.Reglas;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Reglas
{
    public class ExportarArchivosReglas : IExportarArchivosReglas
    {
        public byte[] ExportExel<T>(IEnumerable<T> data)
        {
            using (var ms = new MemoryStream())
            {
                // Crear documento Excel
                using (var spreadsheet = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = spreadsheet.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new SheetData();
                    worksheetPart.Worksheet = new Worksheet(sheetData);

                    var sheets = spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
                    sheets.Append(new Sheet()
                    {
                        Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Inventario"
                    });

                    // Crear fila de encabezados
                    var propiedades = typeof(T).GetProperties();
                    var headerRow = new Row();
                    foreach (var prop in propiedades)
                    {
                        headerRow.Append(new Cell()
                        {
                            DataType = CellValues.String,
                            CellValue = new CellValue(prop.Name)
                        });
                    }
                    sheetData.AppendChild(headerRow);

                    // Crear filas con los datos
                    foreach (var item in data)
                    {
                        var newRow = new Row();
                        foreach (var prop in propiedades)
                        {
                            var valor = prop.GetValue(item)?.ToString() ?? "";
                            newRow.Append(new Cell()
                            {
                                DataType = CellValues.String,
                                CellValue = new CellValue(valor)
                            });
                        }
                        sheetData.AppendChild(newRow);
                    }

                    workbookPart.Workbook.Save();
                }

                return ms.ToArray();
            }
        }

        public byte[] ExportPdf<T>(IEnumerable<T> data)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><head><meta charset='utf-8'><title>Inventario</title></head><body>");
            sb.AppendLine("<h2>REPORTE DE INVENTARIO</h2>");
            sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
            sb.AppendLine("<tr><th>ID</th><th>Nombre</th><th>Marca</th><th>Precio</th></tr>");

            foreach (var p in data)
            {
                var id = p.GetType().GetProperty("IdProducto")?.GetValue(p);
                var nombre = p.GetType().GetProperty("Nombre")?.GetValue(p);
                var marca = p.GetType().GetProperty("Marca")?.GetValue(p);
                var precio = p.GetType().GetProperty("Precio")?.GetValue(p);

                sb.AppendLine($"<tr><td>{id}</td><td>{nombre}</td><td>{marca}</td><td>{precio}</td></tr>");
            }

            sb.AppendLine("</table></body></html>");

            
            

            return  Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
