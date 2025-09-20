using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
	public class Categorias
	{
		public class CategoriasBase
		{
			public string Nombre { get; set; }
			public string Descripcion { get; set; }
            public string Icono { get; set; }
            public DateTime FechaCreacion { get; set; }
		}
        public class CategoriasRequestPadre : CategoriasBase
        {
            public int EstadoId { get; set; }
        }
        public class CategoriasRequestHija : CategoriasBase
		{
			public int EstadoId { get; set; }
			public Guid? PadreId { get; set; }
        }

		public class CategoriasResponse : CategoriasBase
		{
			public Guid CategoriasId { get; set; }
			public Guid PadreId { get; set; }
            public string NombrePadre { get; set; }
            public string Estado { get; set; }
		}

        public class VerificarCategoriaResponse
        {
            public Guid IdCategoria { get; set; }
            public bool EsPadre { get; set; }
            public int CantidadHijas { get; set; }
            public Guid? PadreId { get; set; }
            public bool PadreActivo { get; set; }
        }
        public class CategoriaFlat
        {
            public string PadreNombre { get; set; }
            public string PadreIcono { get; set; }
            public Guid? HijaId { get; set; }
            public Guid? HijaPadreId { get; set; }
            public string HijaNombre { get; set; }
            public string HijaDescripcion { get; set; }
            public string HijaIcono { get; set; }
            public Guid PadreId { get; set; }
        }

        public class CategoriaPadreConHijas
        {
            public string PadreNombre { get; set; }
            public Guid PadreId { get; set; }
            public string PadreIcono { get; set; }
            public List<CategoriasResponse> Hijas { get; set; } 
        }

    }
}
