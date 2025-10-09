using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IResetPasswordDA
    {
        Task<int> GuardarResetPasswordToken(ResetPasswordToken resetPasswordToken);
        Task<int> ResetPassword(ResetPassword resetPasword);
    }
}
