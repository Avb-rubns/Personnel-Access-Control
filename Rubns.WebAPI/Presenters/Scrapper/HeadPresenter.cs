using Rubns.Application.Interface.Scrapper.Queries;

namespace Rubns.WebAPI.Presenters.Scrapper
{
    internal class HeadPresenter : IGetHeadOutputPort
    {
        public List<Dictionary<string, string>> Result { get; private set; }

        public Task Success(List<Dictionary<string, string>> head)
        {
            Result = head;
            return Task.CompletedTask;
        }
    }
}
