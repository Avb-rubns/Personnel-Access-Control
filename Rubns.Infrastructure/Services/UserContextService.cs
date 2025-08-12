namespace Rubns.Infrastructure.Services
{
    internal class UserContextService : IUserContextService
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
    }
}
