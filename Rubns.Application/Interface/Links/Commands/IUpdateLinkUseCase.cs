namespace Rubns.Application.Interface.Links.Commands
{
    public interface IUpdateLinkUseCase
    {
        Task ExecuteAsync(JsonPatchDocument<LinkDTO> link);
    }
}
