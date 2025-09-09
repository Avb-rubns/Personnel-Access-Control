namespace Rubns.Application.Interface.Links.Get
{
    public interface IGetLinkforSlugOutputport : IPresenter<string>
    {
        Task Handler(string urlDestinecion);
    }
}
