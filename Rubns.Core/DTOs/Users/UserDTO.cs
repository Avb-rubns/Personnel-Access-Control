namespace Rubns.Core.DTOs.Users
{
    public class UserDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Value { get; set; }
        public int LevelPermission { get; set; }
        public short Status { get; set; }
    }
}
