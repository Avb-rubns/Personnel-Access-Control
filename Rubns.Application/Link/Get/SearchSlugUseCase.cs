namespace Rubns.Application.Link.Get
{
    internal class SearchSlugUseCase(ILinkRepositoryEFC repositoryEFC
        , ILogger logger
        , IUtils utils)
        : ISearchSlugPort
    {
        private readonly ILinkRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IUtils _utils = utils;
        public async Task<string> SearchSlugAsync(string query, bool? check = false)
        {
            string result = string.Empty;
            try
            {
                if (check.HasValue && check.Value)
                {
                    query = _utils.CreateSlug(query);
                }
                result = await _repositoryEFC.FindSlugAsync(query);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error SearchSlugAsync:{error}", e.Message);
            }

            return result;
        }
    }
}
