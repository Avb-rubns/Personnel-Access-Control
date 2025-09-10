namespace Rubns.Application.Interface.Tickets.Post
{
    public interface ICheckPort
    {
        Task CheckAsync(CheckDTO checkTicket, string op);
    }
}
