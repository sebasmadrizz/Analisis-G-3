using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IAutenticacionFlujo
    {
        Task<Token> LoginAsync(Login login);
    }
}
