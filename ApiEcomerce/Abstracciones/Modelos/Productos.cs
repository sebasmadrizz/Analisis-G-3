using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class ProductosBase
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s\-\,\.\']+$", ErrorMessage = "El nombre contiene caracteres inválidos.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La marca es requerida.")]
        [StringLength(50, ErrorMessage = "La marca no puede tener más de 50 caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s\-]+$", ErrorMessage = "La marca contiene caracteres inválidos.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El precio es requerido.")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        [StringLength(1000, ErrorMessage = "La descripción no puede tener más de 1000 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El stock es requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número positivo.")]
        public int Stock { get; set; }

        
        public string? ImagenUrl { get; set; }

        
        public DateTime? FechaCreacion { get; set; }
        
    }

    public class ProductosRequest : ProductosBase
    {
        [Required(ErrorMessage = "El proveedor es requerido.")]
        public Guid IdProveedor { get; set; }

        [Required(ErrorMessage = "La categoría es requerida.")]
        public Guid IdCategoria { get; set; }
        public Guid? IdProducto { get; set; }
        public string? NombreProveedor { get; set; }
        public string? Categoria { get; set; }


        public int? IdEstado { get; set; }
    }

    public class ProductosResponse : ProductosBase
    {
        public Guid IdProducto { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es requerido.")]
        public string NombreProveedor { get; set; }

        [Required(ErrorMessage = "La categoría es requerida.")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "El estado es requerido.")]
        public string Estado { get; set; }
        public Guid? IdCategoria { get; set; }
        public Guid? IdProveedor { get; set; }
        public int Score{ get; set; }
    }

    public class ProductoConImagenRequest
    {
        [Required(ErrorMessage = "La información del producto es requerida.")]
        public ProductosRequest Productos { get; set; }

        
        public Documento? Imagen { get; set; }
    }








}
