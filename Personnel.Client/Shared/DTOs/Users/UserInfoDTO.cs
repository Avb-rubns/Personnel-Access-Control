namespace Personnel.Client.Shared.DTOs.Users
{
    public class UserInfoDTO
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ID { get; set; }
        public string Role { get; set; }
        public string RolID { get; set; }
        public bool Status { get; set; }
        public long Expiration { get; set; }

    }
}
