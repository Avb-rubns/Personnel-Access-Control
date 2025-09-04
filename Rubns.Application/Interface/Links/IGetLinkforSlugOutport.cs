namespace Rubns.Application.Interface.Links
{
    public interface IGetLinkforSlugOutport : IPresenter<string>
    {
        Task Handler(string urlDestinecion);
    }
}
