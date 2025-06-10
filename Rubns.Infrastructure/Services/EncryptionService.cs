
namespace Rubns.Infrastructure.Services
{
    internal class EncryptionService : IEncryptionService
    {
        IConfiguration Configuration { get; }
        public EncryptionService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GenerateApiKey(RegisterDTO register)
        {
            string apiKey = string.Empty;

            string wordSecret = Configuration["WordSecret"];

            byte[] salt1 = Encoding.UTF8.GetBytes(wordSecret);
            byte[] salt2 = Encoding.UTF8.GetBytes(register.WordSecretUser);

            byte[] combinedSalt = salt1.Concat(salt2).ToArray();

            byte[] clientData = Encoding.UTF8.GetBytes(register.NameApp);

            using (var hmacsha256 = new HMACSHA256(combinedSalt))
            {
                byte[] hash = hmacsha256.ComputeHash(clientData);
                apiKey = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            return apiKey;

        }

        public bool ValidatePass(string pass, string passUser)
        {
            bool result = false;
            string Salt = Configuration["WordSecretPass"];

            byte[] salt = Encoding.UTF8.GetBytes(Salt);

            byte[] userPass = Encoding.UTF8.GetBytes(pass);

            using (var hmacsha256 = new HMACSHA256(salt))
            {
                byte[] hash = hmacsha256.ComputeHash(userPass);
                var passHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

                result = passUser.Equals(passHash);
            }

            return result;
        }

        public string GeneratePassTemp(RegisterUserDTO register)
        {
            string result = string.Empty;
            string Salt = Configuration["WordSecretPass"];

            byte[] salt = Encoding.UTF8.GetBytes(Salt);

            string[] temp = register.Email.Split('@');

            byte[] userPass = Encoding.UTF8.GetBytes(temp[0]);

            using (var hmacsha256 = new HMACSHA256(salt))
            {
                byte[] hash = hmacsha256.ComputeHash(userPass);
                var passHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

                result = passHash;

            }


            return result;
        }
    }
}