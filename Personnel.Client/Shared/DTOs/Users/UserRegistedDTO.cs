namespace Personnel.Client.Shared.DTOs.Users
{
    public class UserRegistedDTO
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int RolID { get; set; }
        public int LevelPermission { get; set; }
        public bool Status { get; set; }
    }
}
