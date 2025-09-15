namespace Rubns.Application.Interface.Tickets.Commands
{
    public interface ICreateCheckUseCase
    {
        Task ExecuteAsync(CheckDTO checkTicket, string op);
    }
}
