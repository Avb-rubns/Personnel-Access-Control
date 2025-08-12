namespace Rubns.Core.Services
{
    public interface IUserContextService
    {
        string UserId { get; set; }
        List<string> Roles { get; set; }
        string Status { get; set; }
        string Name { get; set; }
    }
}
