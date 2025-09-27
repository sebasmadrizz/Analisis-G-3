using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class ResetPasswordDA: IResetPasswordDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public ResetPasswordDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<int> GuardarResetPasswordToken(ResetPasswordToken resetPasswordToken)
        {
            string query = @"GUARDAR_RESETPASSWORDTOKEN";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Id= Guid.NewGuid(),
                UserId= resetPasswordToken.UserId,
                TokenHash= resetPasswordToken.TokenHash,
                ExpiraEn= resetPasswordToken.ExpiraEn,
                Usado= false
            });
            return resultadoConsulta;
        }

        public async Task<int> ResetPassword(ResetPassword resetPasword)
        {
            string query = @"RESETPASSWORD";
            var resultadoConsulta = await _sqlConnection.ExecuteScalarAsync<int>(query, new
            {
                Email = resetPasword.Email,
                Password = resetPasword.Password,
                Token = resetPasword.Token
            });
            return resultadoConsulta;

        }
    }
}
