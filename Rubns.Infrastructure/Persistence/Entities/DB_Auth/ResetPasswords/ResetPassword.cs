namespace Rubns.Infrastructure.Persistence.Entities.DB_Auth.ResetPasswords
{
    public class ResetPassword
    {
        public int ResetPasswordID { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Registed { get; set; }
    }
}
