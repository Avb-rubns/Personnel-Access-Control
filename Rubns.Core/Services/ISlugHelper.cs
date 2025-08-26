namespace Rubns.Core.Services
{
    public interface ISlugHelper
    {
        Task<string> GenerateUniqueSlugAsync(string nameOrSlug);
        string GenerateShortHash(string input);
    }
}
