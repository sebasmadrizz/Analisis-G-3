using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos.Categoria
{
    public class Categoria
    {
        public Guid categoriasId { get; set; }
        public Guid? padreId { get; set; }

        public string estado { get; set; }

        [Required]
        public string nombre { get; set; }

        public string nombrePadre { get; set; }
        [Required]
        public string descripcion { get; set; }
        public DateTime fechaCreacion { get; set; }
    }


    public class VerificarCategoriaResponse
    {
        public Guid IdCategoria { get; set; }
        public bool EsPadre { get; set; }
        public int CantidadHijas { get; set; }
        public Guid? PadreId { get; set; }
        public bool PadreActivo { get; set; }
    }
}
