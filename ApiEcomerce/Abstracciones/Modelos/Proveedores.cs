using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
    public class Proveedores
    {
        [Required]
        public Guid PROVEEDOR_ID { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
        public string Nombre_PROVEEDOR { get; set; }
        [Required(ErrorMessage = "El correo electronico es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electronico valido")]
        public string Correo_ELECTRONICO { get; set; }
        [Required(ErrorMessage = "El tipo es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]

        public string TIPO { get; set; }
        [Required(ErrorMessage = "La direccion es requerida")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "El telefono  es requerido")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe tener exactamente 8 dígitos")]
        public string Telefono { get; set; }
        [Required]
        public int ESTADO_ID { get; set; }
        [Required]
        public DateTime Fecha_Registro { get; set; }
        [Required(ErrorMessage = "El nombre de contacto es requerido")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo se permiten letras y espacios")]

        public string Nombre_Contacto { get; set; }
        //falta el join para el estado
    }
}
