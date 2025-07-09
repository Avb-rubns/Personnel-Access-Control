namespace Personnel.Client.Shared.DTOs.LogIn
{
    public class UserInfoDTO
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public int ID { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
        public long Expiration { get; set; }

    }
}
