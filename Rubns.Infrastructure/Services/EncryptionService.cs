namespace Rubns.Infrastructure.Services
{
    internal class EncryptionService : IEncryptionService
    {
        private readonly IConfiguration Configuration;

        public EncryptionService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GenerateApiKey(RegisterDTO register)
        {
            string wordSecret = Configuration["WordSecret"];
            byte[] salt1 = Encoding.UTF8.GetBytes(wordSecret);
            byte[] salt2 = Encoding.UTF8.GetBytes(register.WordSecretUser);
            byte[] combinedSalt = salt1.Concat(salt2).ToArray();

            return ComputeHash(register.NameApp, combinedSalt);
        }

        public bool ValidatePass(string pass, string passUser)
        {
            string salt = Configuration["WordSecretPass"];
            string hashed = ComputeHash(pass, Encoding.UTF8.GetBytes(salt));
            return passUser.Equals(hashed);
        }

        public string GeneratePassTemp(RegisterUserDTO register)
        {
            string salt = Configuration["WordSecretPass"];
            string baseString = register.Email.Split('@')[0];
            return ComputeHash(baseString, Encoding.UTF8.GetBytes(salt));
        }

        public string GenerateTokenForgotPass(string email)
        {
            string salt = Configuration["WordSecretForgotPass"];
            return ComputeHash(email, Encoding.UTF8.GetBytes(salt));
        }

        public string GenerateNewPass(string newPass)
        {
            string salt = Configuration["WordSecretPass"];
            return ComputeHash(newPass, Encoding.UTF8.GetBytes(salt));
        }

        private string ComputeHash(string input, byte[] salt)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);

            using var hmac = new HMACSHA256(salt);
            byte[] hash = hmac.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
