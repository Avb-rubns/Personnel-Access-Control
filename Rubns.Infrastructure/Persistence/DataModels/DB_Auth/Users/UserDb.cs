namespace Rubns.Infrastructure.Persistence.DataModels.DB_Auth.Users
{
    public class UserDb
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int? RolID { get; set; }
        public bool Status { get; set; }
        public DateTime Registed { get; set; }
    }
}
