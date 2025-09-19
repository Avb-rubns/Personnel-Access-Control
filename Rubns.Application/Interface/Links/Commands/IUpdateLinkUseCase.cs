namespace Rubns.Application.Interface.Links.Commands
{
    public interface IUpdateLinkUseCase
    {
        Task ExecuteAsync(string id, JsonPatchDocument<LinkUpdateDTO> link);
    }
}
