namespace Rubns.Core.Entities.Auth
{
    public class ResetPassword
    {
        public int ResetPasswordID { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Registed { get; set; }
    }
}
