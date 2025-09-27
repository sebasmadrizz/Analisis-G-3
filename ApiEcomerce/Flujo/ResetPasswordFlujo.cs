using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ResetPasswordFlujo: IResetPasswordFlujo
    {
        private readonly IResetPasswordDA _resetPasswordDA;
        public ResetPasswordFlujo(IResetPasswordDA resetPasswordDA)
        {
            _resetPasswordDA = resetPasswordDA;
        }

        public async Task<int> GuardarResetPasswordToken(ResetPasswordToken resetPasswordToken)
        {
            return await _resetPasswordDA.GuardarResetPasswordToken(resetPasswordToken);
        }

        public async Task<int> ResetPassword(ResetPassword resetPasword)
        {
            return await _resetPasswordDA.ResetPassword(resetPasword);
        }
    }
}
