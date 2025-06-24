namespace Rubns.Core.Ticket
{
    public interface ITicketRepository
    {
        Task<int> InsertCheckAsync(CheckInDTO check, int userID);
    }
}
