namespace Rubns.Infrastructure.Persistence.DataModels.DB_Auth.ResetPasswords
{
    public class ResetPasswordDb
    {
        public int ResetPasswordID { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Registed { get; set; }
    }
}
