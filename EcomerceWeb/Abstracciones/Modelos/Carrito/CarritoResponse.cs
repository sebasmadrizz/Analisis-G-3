using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos.Carrito
{
    public class CarritoResponse
    {

       
       
            public Guid CarritoId { get; set; }
            public decimal Total { get; set; }
            public DateTime FechaCreacion { get; set; }
            public List<CarritoProductoResponse> Productos { get; set; } = new();
        
    }
}

