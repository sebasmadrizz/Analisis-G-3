using Abstracciones.Modelos;
namespace Abstracciones.Interfaces.Reglas
{
    public interface IAutenticacionReglas
    {
        Task<Token> LoginAsync(Login login);

    }
}
