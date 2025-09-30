using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IResetPasswordFlujo
    {
        Task<int> GuardarResetPasswordToken(ResetPasswordToken resetPasswordToken);
        Task<int> ResetPassword(ResetPassword resetPasword);
    }
}
