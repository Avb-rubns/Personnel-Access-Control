namespace Rubns.Infrastructure.Persistence.DataModels.DB_Auth.StoredProcedures
{
    public class UserWithRolInfoSpResultDb
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Value { get; set; }
        public int RolId { get; set; }
        public int LevelPermission { get; set; }
        public bool Status { get; set; }
        public string Phone { get; set; }
    }
}
