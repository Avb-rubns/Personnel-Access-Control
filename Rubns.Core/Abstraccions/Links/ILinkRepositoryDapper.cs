namespace Rubns.Core.Abstraccions.Links
{
    public interface ILinkRepositoryDapper
    {
        Task<int> CountLinks(string filter, string search);

    }
}
