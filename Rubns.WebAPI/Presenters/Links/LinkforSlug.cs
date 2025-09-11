using Rubns.Application.Interface.Links.Queries;

namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinkforSlug : IGetLinkbySlugOutputPort
    {
        public string Result { get; set; }

        public Task Success(string urlDestinecion)
        {
            Result = urlDestinecion;
            return Task.CompletedTask;
        }
    }
}
