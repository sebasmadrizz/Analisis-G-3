using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
    public class Paginacion<T>(List<T> items,int pageIndex,int totalPages)
    {
        public List<T> Items { get; set; } = items;
        public int PageIndex { get;} = pageIndex;
        public int TotalPages { get; } = totalPages;
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;


    }
}
