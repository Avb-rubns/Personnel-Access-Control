using Rubns.Application.Interface.Scrapper.Queries;

namespace Rubns.WebAPI.Presenters.Scrapper
{
    internal class ITitlePresenter : IGetTitleOutputPort
    {
        public Dictionary<string, string> Result { get; private set; }

        public Task Success(Dictionary<string, string> titles)
        {
            Result = titles;
            return Task.CompletedTask;
        }
    }
}
