namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinkbySlugOutputPort : IPresenter<string>
    {
        Task Success(string urlDestinecion);
    }
}
