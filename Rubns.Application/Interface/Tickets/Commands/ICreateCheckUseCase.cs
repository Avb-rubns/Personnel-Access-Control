namespace Rubns.Application.Interface.Tickets.Post
{
    public interface ICreateCheckUseCase
    {
        Task ExecuteAsync(CheckDTO checkTicket, string op);
    }
}
