namespace Rubns.Application.Interface.Links.Get
{
    public interface ISearchSlugPort
    {
        Task SearchSlugAsync(string query, bool? check = false);
    }
}
