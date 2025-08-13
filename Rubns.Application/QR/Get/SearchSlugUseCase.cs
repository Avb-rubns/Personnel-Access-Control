namespace Rubns.Application.QR.Get
{
    internal class SearchSlugUseCase(IQRRepositoryEFC repositoryEFC
        , ILogger logger)
        : ISearchSlugPort
    {
        private readonly IQRRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;
        public async Task<string> SearchSlugAsync(string query)
        {
            string result = string.Empty;

            try
            {

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
