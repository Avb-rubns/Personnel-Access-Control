
namespace Rubns.Core.Ticket
{
    public interface ICheckInPort<T>
    {
        Task<T> CheckIn(CheckInDTO checkTicket);
    }
}
