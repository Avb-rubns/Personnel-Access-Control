namespace Personnel.Client.Shared.DTOs.Auth
{
    public class ResetPasswordDTO
    {
        public int ResetPasswordID { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Registed { get; set; }
    }
}
