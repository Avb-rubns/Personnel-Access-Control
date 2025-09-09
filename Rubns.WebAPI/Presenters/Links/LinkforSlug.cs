using Rubns.Application.Interface.Links.Get;

namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinkforSlug : IGetLinkforSlugOutputport
    {
        public string Content { get; set; }

        public Task Handler(string urlDestinecion)
        {
            Content = urlDestinecion;
            return Task.CompletedTask;
        }
    }
}
