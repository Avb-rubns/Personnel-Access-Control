namespace Rubns.Application.UseCases.Links.Commands
{
    internal class UpdateLinkUseCase : IUpdateLinkUseCase
    {
        public Task ExecuteAsync(JsonPatchDocument<LinkDTO> link)
        {
            throw new NotImplementedException();
        }
    }
}
