namespace Rubns.Core.Ticket
{
    public interface ICheckPort<T>
    {
        Task<T> CheckAsync(CheckDTO checkTicket, string op);
    }
}
