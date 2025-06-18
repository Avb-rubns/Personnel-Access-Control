namespace Personnel.Client.Shared.DTOs.LogIn
{
    public class AuthResponseDTO
    {
        public JWT AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expiration { get; set; }
    }
}
