namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinkPresenter : IGetLinkBySlugOutputPort
    {
        public LinkDTO Result { get; private set; }

        public Task Success(Link link)
        {



            return Task.CompletedTask;
        }
    }
}
