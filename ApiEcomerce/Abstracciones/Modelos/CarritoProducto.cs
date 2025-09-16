using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
	public class CarritoProducto
	{
		public class CarritoProductoBase
		{
			public Guid CarritoProductoId { get; set; }

			public Guid CarritoId { get; set; }

			public int Cantidad { get; set; }

			public decimal TotalLinea { get; set; }

			public Guid ProductosId { get; set; }


		}

		public class CarritoProductoRequest
		{
			public Guid CarritoId { get; set; }
			public int Cantidad { get; set; }
			public Guid ProductosId { get; set; }
		}

		public class CarritoProductoResponse
		{

			public Guid CarritoProductoId { get; set; }
			public Guid CarritoId { get; set; }
			public Guid ProductosId { get; set; }
			public int Cantidad { get; set; }
			public decimal TotalLinea { get; set; }
			public string NombreProducto { get; set; }
			public decimal PrecioUnitario { get; set; }
			public string ImagenUrl { get; set; }
			public string Descripcion { get; set; }
            public int StockDisponible { get; set; }
        }
	}
}
