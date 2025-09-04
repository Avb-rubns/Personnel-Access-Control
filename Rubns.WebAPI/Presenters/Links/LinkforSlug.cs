namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinkforSlug : IGetLinkforSlugOutport
    {
        public string Content { get; set; }

        public Task Handler(string urlDestinecion)
        {
            Content = urlDestinecion;
            return Task.CompletedTask;
        }
    }
}
