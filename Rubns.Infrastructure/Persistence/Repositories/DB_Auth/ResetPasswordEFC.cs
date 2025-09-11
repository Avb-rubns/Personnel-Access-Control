namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class ResetPasswordEFC(AuthDbContextEFC context) : IResetPasswordRepositoryEFC
    {
        AuthDbContextEFC _contextEFC = context;
        public async Task<int> AddAsync(ResetPassword reset)
        {
            ResetPasswordDb resetPassword = new()
            {
                UserId = reset.UserId,
                Token = reset.Token,

            };

            await _contextEFC.ResetPasswords.AddAsync(resetPassword);

            return await _contextEFC.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(ResetPassword reset)
        {
            ResetPasswordDb resetPassword = new()
            {
                ResetPasswordID = reset.ResetPasswordID,
                UserId = reset.UserId,
                Token = reset.Token,

            };

            _contextEFC.ResetPasswords.Remove(resetPassword);

            return await _contextEFC.SaveChangesAsync();
        }

        public async Task<ResetPassword> FindbyTokenAsync(string token)
        {
            ResetPassword resetPasswordDTO = new();


            var reset = await _contextEFC.ResetPasswords
                .AsNoTracking()
                .FirstOrDefaultAsync(rp => rp.Token == token);


            if (reset is { UserId: > 0 })
            {
                resetPasswordDTO.ResetPasswordID = reset.ResetPasswordID;
                resetPasswordDTO.UserId = reset.UserId;
                resetPasswordDTO.Token = token;
                resetPasswordDTO.Registed = reset.Registed;
            }

            return resetPasswordDTO;
        }

        public async Task<bool> FindByUserIdAsync(int userID)
        {

            var resetPassword = await _contextEFC.ResetPasswords
                .AsNoTracking()
                .FirstOrDefaultAsync(rp => rp.UserId == userID);

            return resetPassword is { ResetPasswordID: > 0 };

        }
    }
}
