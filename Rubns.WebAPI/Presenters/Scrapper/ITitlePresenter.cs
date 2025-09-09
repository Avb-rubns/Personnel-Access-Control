namespace Rubns.WebAPI.Presenters.Scrapper
{
    internal class ITitlePresenter : ITitleOutputPort
    {
        public Dictionary<string, string> Content { get; private set; }

        public Task Handler(Dictionary<string, string> titles)
        {
            Content = titles;
            return Task.CompletedTask;
        }
    }
}
