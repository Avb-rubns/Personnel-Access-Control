namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetURLDestinationBySlugOutputPort : IPresenter<string>
    {
        Task Success(string urlDestinecion);
    }
}
