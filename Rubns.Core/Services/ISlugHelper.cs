namespace Rubns.Core.Services
{
    public interface ISlugHelper
    {
        Task<string> GenerateUniqueSlugByNameOrSlugAsync(string nameOrSlug);
        string GenerateShortHash(string input);
    }
}
