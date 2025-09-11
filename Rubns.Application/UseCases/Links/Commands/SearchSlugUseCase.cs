namespace Rubns.Application.UseCases.Links.Commands
{
    internal class SearchSlugUseCase(ILinkRepositoryEFC repositoryEFC
        , ILogger logger
        , IUtils utils)
        : ISearchSlugUseCase
    {
        private readonly ILinkRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IUtils _utils = utils;
        public async Task SearchSlugAsync(string query, bool? check = false)
        {
            string result = string.Empty;
            try
            {
                if (check.HasValue && check.Value)
                {
                    query = _utils.CreateSlug(query);
                }

                result = await _repositoryEFC.FindBySlugAsync(query);

                if (string.IsNullOrEmpty(result))
                {
                    throw new NotFoundException("No existe el slug.", "El elemento no existe");
                }

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error SearchSlugAsync:{error}", e.Message);
                throw;
            }

        }
    }
}
