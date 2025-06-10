namespace Rubns.Core.DTOs.LogOut
{
    public class SessionUserDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
