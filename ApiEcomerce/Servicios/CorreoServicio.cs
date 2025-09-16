using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Abstracciones.Interfaces.Servicios;
using static Abstracciones.Modelos.Carrito;

public class CorreoServicio : ICorreoServicio
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _usuario;
    private readonly string _password;
    private readonly string _from;

    public CorreoServicio(string smtpServer, int smtpPort, string usuario, string password, string from)
    {
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _usuario = usuario;
        _password = password;
        _from = from;
    }

    public async Task EnviarCorreoCarritoAsync(CarritoCorreo carrito)
    {
        var html = GenerarHtmlCorreo(carrito);

        using var mensaje = new MailMessage();
        mensaje.From = new MailAddress(_from);
        mensaje.To.Add(carrito.CorreoElectronico);
        mensaje.Subject = "Resumen de tu pedido";
        mensaje.Body = html;
        mensaje.IsBodyHtml = true;

        using var cliente = new SmtpClient(_smtpServer, _smtpPort)
        {
            Credentials = new NetworkCredential(_usuario, _password),
            EnableSsl = true
        };

        await cliente.SendMailAsync(mensaje);
    }

    private string GenerarHtmlCorreo(CarritoCorreo carrito)
    {
        var html = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        table {{ border-collapse: collapse; width: 100%; }}
        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        th {{ background-color: #f2f2f2; }}
        .total {{ font-weight: bold; }}
    </style>
</head>
<body>
    <h2>Resumen de tu pedido</h2>
    <p>Hola {carrito.NombreUsuario} {carrito.Apellido},</p>
    <p>Gracias por tu compra. Aquí está el detalle de tu pedido:</p>

    <table>
        <thead>
            <tr>
                <th>Producto</th>
                <th>Marca</th>
                <th>Precio</th>
                <th>Cantidad</th>
                <th>Total</th>
            </tr>
        </thead>
        <tbody>";

        foreach (var producto in carrito.Productos)
        {
            html += $@"
            <tr>
                <td>{producto.NombreProducto}</td>
                <td>{producto.Marca}</td>
                <td>${producto.Precio:F2}</td>
                <td>{producto.Cantidad}</td>
                <td>${producto.TotalLinea:F2}</td>
            </tr>";
        }

        html += $@"
        </tbody>
    </table>

    <p class='total'>Total carrito: ${carrito.TotalCarrito:F2}</p>
    <p>Fecha de creación: {carrito.FechaCreacion:dd/MM/yyyy HH:mm}</p>

    <p>¡Gracias por comprar con nosotros!</p>
</body>
</html>";

        return html;
    }
}