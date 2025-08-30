namespace Rubns.Application.Interface.Links
{
    public interface IGetLinksOurPort : IPresenter<TableLinkDTO>
    {
        Task Handler(List<LinkWithClicks> links, int total);
    }
}
