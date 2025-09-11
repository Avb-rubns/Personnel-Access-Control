namespace Rubns.Core.Abstraccions.Tickets
{
    public interface ITicketRepository
    {
        Task<int> InsertAsync(CheckDTO check, int userID, string proc);
    }
}
