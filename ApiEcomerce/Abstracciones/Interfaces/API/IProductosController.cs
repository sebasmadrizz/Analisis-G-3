using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Interfaces.API
{
    public interface IProductosController
    {
        Task<IActionResult> ObtenerProductosIndex();
        Task<IActionResult> ExportExelPorCategoria(Guid categoriaId);
        Task<IActionResult> ExportPdfPorCategoria(Guid categoriaId);
        Task<IActionResult> ObtenerProductosBuscadosFTS(int PageIndex, int PageSize, string searchTerm);

        Task<IActionResult> Obtener();
        Task<IActionResult> ObtenerProductosBuscados(string nombre);
        Task<IActionResult> ObtenerProductosXCategoria(Guid idCategoria, int pageIndex, int pageSize);
        Task<IActionResult> ObtenerPorId(Guid IdProducto);

        Task<IActionResult> Agregar(ProductoConImagenRequest request);

        Task<IActionResult> Editar(Guid IdProducto, ProductoConImagenRequest request);

        Task<IActionResult> Eliminar(Guid IdProducto);
        Task<IActionResult> ListarProductosPaginado(int pageIndex, int pageSize);
         Task<IActionResult> ExportExel();
         Task<IActionResult> ExportPdf();
    }
}
