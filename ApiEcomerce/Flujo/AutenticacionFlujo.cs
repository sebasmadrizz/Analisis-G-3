using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;


namespace Reglas
{
    public class AutenticacionFlujo: IAutenticacionFlujo
    {
        private IAutenticacionReglas _autenticacionReglas;

        public AutenticacionFlujo(IAutenticacionReglas autenticacionReglas)
        {
            _autenticacionReglas = autenticacionReglas;
        }

        public async Task<Token> LoginAsync(Login login)
        {
            return await _autenticacionReglas.LoginAsync(login);
        }

    }
}
