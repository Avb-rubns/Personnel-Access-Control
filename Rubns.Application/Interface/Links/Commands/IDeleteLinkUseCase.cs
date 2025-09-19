namespace Rubns.Application.Interface.Links.Commands
{
    public interface IDeleteLinkUseCase
    {
        Task ExecuteAsync(string id);
    }
}
