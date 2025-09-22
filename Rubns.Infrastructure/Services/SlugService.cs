namespace Rubns.Infrastructure.Services
{
    internal class SlugService(IUtils utils
        , ILinkRepositoryEFC linkRepositoryEFC)
        : ISlugHelper
    {
        private readonly IUtils _utils = utils;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;

        private const int MaxAttempts = 100;
        public string GenerateShortHash(string input)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(bytes, 0, 4).Replace("-", "").ToLower();
        }

        public async Task<string> GenerateUniqueSlugByNameOrSlugAsync(string nameOrSlug)
        {
            if (string.IsNullOrWhiteSpace(nameOrSlug))
                throw new ArgumentException("El valor no puede estar vacío.", nameof(nameOrSlug));

            string baseSlug = _utils.CreateSlug(nameOrSlug);
            string uniqueSlug = baseSlug;

            for (int counter = 0; counter < MaxAttempts; counter++)
            {
                if (counter > 0)
                {
                    uniqueSlug = $"{baseSlug}-{counter}";
                }

                if (string.IsNullOrEmpty(await _linkRepositoryEFC.FindBySlugAsync(uniqueSlug)))
                {
                    return uniqueSlug;
                }
            }

            string hash = GenerateShortHash(baseSlug);
            uniqueSlug = $"{baseSlug}-{hash}";
            return uniqueSlug;

        }
    }
}
