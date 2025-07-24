namespace Personnel.Client.Shared.DTOs.Users
{
    public class UserInfoDTO
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int ID { get; set; }
        public string Role { get; set; }
        public int RolID { get; set; }
        public bool Status { get; set; }
        public long Expiration { get; set; }

    }
}
