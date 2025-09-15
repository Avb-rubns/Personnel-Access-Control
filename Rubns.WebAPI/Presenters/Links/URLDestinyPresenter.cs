namespace Rubns.WebAPI.Presenters.Links
{
    internal class URLDestinyPresenter : IGetURLDestinationBySlugOutputPort
    {
        public string Result { get; set; }

        public Task Success(string urlDestinecion)
        {
            Result = urlDestinecion;
            return Task.CompletedTask;
        }
    }
}
