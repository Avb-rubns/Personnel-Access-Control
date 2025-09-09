namespace Rubns.WebAPI.Presenters.Scrapper
{
    internal class MetasPresenter : IMetasOutputPort
    {
        public Dictionary<string, string> Content { get; private set; }
        public Task Handeler(Dictionary<string, string> metas)
        {
            Content = metas;
            return Task.CompletedTask;
        }
    }
}
