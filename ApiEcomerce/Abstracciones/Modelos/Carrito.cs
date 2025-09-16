using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
    public class Carrito
    {


        public class CarritoBase
        {
            public Guid CarritoId { get; set; }
            public Guid UsuarioId { get; set; }
            public DateTime FechaCreacion { get; set; }
            public decimal Total { get; set; }
        }

        /*public class CarritoRequest
        {

        }*/
        //en teoria no se ocupa porque viene en claims

        public class CarritoResponse
        {
            public Guid CarritoId { get; set; }
            public decimal Total { get; set; }
            public DateTime FechaCreacion { get; set; }
            public List<CarritoProducto.CarritoProductoResponse> Productos { get; set; }
        }


        public class ProductoCarritoCorreo
        {
            public Guid ProductosId { get; set; }
            public string NombreProducto { get; set; }
            public string Marca { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad { get; set; }
            public decimal TotalLinea { get; set; }
        }


        public class CarritoCorreo
        {
            // Datos del usuario
            public Guid UsuarioId { get; set; }
            public string NombreUsuario { get; set; }
            public string Apellido { get; set; }
            public string CorreoElectronico { get; set; }
            public string Telefono { get; set; }
            public string Direccion { get; set; }

            public Guid CarritoId { get; set; }
            public DateTime FechaCreacion { get; set; }
            public decimal TotalCarrito { get; set; }

            public List<ProductoCarritoCorreo> Productos { get; set; } = new List<ProductoCarritoCorreo>();
        }







    }
}
