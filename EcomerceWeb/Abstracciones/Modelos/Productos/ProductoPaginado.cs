namespace Abstracciones.Modelos.Productos
{

    public class ProductoPaginado
    {
        public List<Producto> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public int PageSize {  get; set; }


    }
    public class ProductosBuscados
    {
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public ProductoPaginado data { get; set; }

        public string suggestion { get; set; }


    }


}
