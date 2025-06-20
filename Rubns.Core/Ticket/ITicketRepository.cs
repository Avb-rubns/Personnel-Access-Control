namespace Rubns.Core.Ticket
{
    public interface ITicketRepository
    {
        Task<int> InsertCheckAsync(CheckDTO check, int userID);
    }
}
