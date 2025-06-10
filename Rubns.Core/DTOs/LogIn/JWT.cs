namespace Rubns.Core.DTOs.Login
{
    public class JWT
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string ExpiresIn { get; set; }
    }
}
