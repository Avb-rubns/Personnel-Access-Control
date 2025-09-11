namespace Rubns.Application.Interface.Clicks.Commands
{
    public interface ICreateClickUseCase
    {
        Task ExecuteAsync(HttpRequest request, int idLink);
    }
}
