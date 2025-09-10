namespace Rubns.Core.Abstraccions.Tickets
{
    public interface ITicketRepository
    {
        Task<int> InsertCheckInAsync(CheckDTO check, int userID);
        Task<int> InsertCheckOutAsync(CheckDTO check, int userID);
    }
}
