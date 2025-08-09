namespace Rubns.Core.Ticket
{
    public interface ICheckOutPort<T>
    {
        Task<T> CheckOut(CheckDTO checkTicket);
    }
}
