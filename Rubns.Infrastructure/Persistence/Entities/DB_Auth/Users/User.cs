namespace Rubns.Infrastructure.Persistence.Entities.DB_Auth.Users
{
    public class User
    {
        public int UserID { get; set; }
        public int Name { get; set; }
        public int Email { get; set; }
        public int Password { get; set; }
        public int? RolID { get; set; }
    }
}
