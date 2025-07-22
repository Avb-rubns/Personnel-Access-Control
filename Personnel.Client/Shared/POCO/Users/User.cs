namespace Personnel.Client.Shared.POCO.Users
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RolID { get; set; }
        public string Phone { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
    }
}
