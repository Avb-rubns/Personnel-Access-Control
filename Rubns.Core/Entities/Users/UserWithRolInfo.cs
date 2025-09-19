namespace Rubns.Core.Entities.Users
{
    public class UserWithRolInfo
    {
        public int UserId { get; set; }
        public string FriendlyUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RolName { get; set; }
        public string Phone { get; set; }
        public int RolId { get; set; }
        public string FriendlyRolId { get; set; }
        public int LevelPermission { get; set; }
        public bool Status { get; set; }
        public DateTimeOffset Registed { get; set; }

    }
}
