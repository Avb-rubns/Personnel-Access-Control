namespace Rubns.Application.Interface.Links.Commands
{
    public interface ISearchSlugUseCase
    {
        Task SearchSlugAsync(string query, bool? check = false);
    }
}
