namespace Personnel.Client.Shared.DTOs.LogIn
{
    public class JWT
    {
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        public string ExpiresIn { get; set; }
    }
}
