namespace Rubns.Core.DTOs.LogIn
{
    public class RefreshTokenResponseDTO
    {
        public JWT Token { get; set; }
        public string RefreshToken { get; set; }
        public long Expiration { get; set; }
    }
}
