using Abstracciones.Interfaces.Reglas;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using iTextSharp.text;
using iTextSharp.text.pdf;
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
                        headerRow.Append(new DocumentFormat.OpenXml.Spreadsheet.Cell()
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
                            newRow.Append(new DocumentFormat.OpenXml.Spreadsheet.Cell()
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
            using var ms = new MemoryStream();

            // Crear documento
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            var writer = PdfWriter.GetInstance(document, ms);
            document.Open();

            // Título
            var titulo = new Paragraph("REPORTE DE INVENTARIO")
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20f
            };
            document.Add(titulo);

            // Tabla con 4 columnas: Id, Nombre, Marca, Precio
            var table = new PdfPTable(4)
            {
                WidthPercentage = 100
            };
            table.SetWidths(new float[] { 1f, 3f, 2f, 2f }); // Anchos proporcionales

            // Encabezados
            table.AddCell("ID");
            table.AddCell("Nombre");
            table.AddCell("Marca");
            table.AddCell("Precio");

            // Filas con datos
            foreach (var p in data)
            {
                var id = p.GetType().GetProperty("IdProducto")?.GetValue(p)?.ToString() ?? "";
                var nombre = p.GetType().GetProperty("Nombre")?.GetValue(p)?.ToString() ?? "";
                var marca = p.GetType().GetProperty("Marca")?.GetValue(p)?.ToString() ?? "";
                var precio = p.GetType().GetProperty("Precio")?.GetValue(p)?.ToString() ?? "";

                table.AddCell(id);
                table.AddCell(nombre);
                table.AddCell(marca);
                table.AddCell(precio);
            }

            document.Add(table);
            document.Close();

            return ms.ToArray();
        }
    }
}
