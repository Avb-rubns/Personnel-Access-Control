namespace Rubns.Infrastructure.Persistence.Entities.DB_Auth.SessionUsers
{
    public class SessionUser
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
