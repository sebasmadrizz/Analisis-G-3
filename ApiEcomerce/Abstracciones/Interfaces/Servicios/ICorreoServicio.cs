using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Abstracciones.Modelos.Carrito;

namespace Abstracciones.Interfaces.Servicios
{
    public interface ICorreoServicio
    {
        Task EnviarCorreoCarritoAsync(CarritoCorreo carrito);
        Task EnviarCorreoRecuperacionAsync(string nombreUsuario, string correoElectronico, string tokenLink);
    }
}
