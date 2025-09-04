namespace Rubns.Application.Link.Delete
{
    internal class LinkUseCase(ILinkRepositoryEFC linkRepositoryEFC
        , ILogger logger)
        : IDeleteLinkPort
    {
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly ILogger _logger = logger;
        public async Task<int> DeleteLinkPortAsync(int id)
        {
            int result = -1;

            try
            {
                var remove = await _linkRepositoryEFC.DeleteLinkAsync(id);

                return remove >= 0 ? 1 : remove;

            }
            catch (Exception e)
            {

                _logger.Error(e, "Error in LinkUseCase:{error}", e.Message);
            }

            return result;
        }
    }
}
