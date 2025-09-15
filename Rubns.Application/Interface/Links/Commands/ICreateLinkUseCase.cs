using Personnel.Client.Shared.DTOs.Link.Commands;

namespace Rubns.Application.Interface.Links.Commands
{
    public interface ICreateLinkUseCase
    {
        Task ExecuteAsync(LinkCreateDTO createDTO);
    }
}
