namespace Rubns.Core.Entities.Users
{
    public class UserClaim
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int ID { get; set; }
        public string FrindlyId { get; set; }
        public string Role { get; set; }
        public int RolID { get; set; }
        public string FrindlyRolId { get; set; }
        public bool Status { get; set; }
        public long Expiration { get; set; }
    }
}
