namespace Rubns.WebAPI.Presenters.Scrapper
{
    internal class HeadPresenter : IHeadOutputPort
    {
        public List<Dictionary<string, string>> Content { get; private set; }

        public Task Handler(List<Dictionary<string, string>> head)
        {
            Content = head;
            return Task.CompletedTask;
        }
    }
}
