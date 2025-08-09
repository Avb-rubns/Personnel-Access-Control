namespace Rubns.Core.Ticket
{
    public interface ITicketRepository
    {
        Task<int> InsertCheckInAsync(CheckDTO check, int userID);
        Task<int> InsertCheckOutAsync(CheckDTO check, int userID);
    }
}
