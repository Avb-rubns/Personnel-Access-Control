using Rubns.Application.Interface.Scrapper.Queries;

namespace Rubns.WebAPI.Presenters.Scrapper
{
    internal class MetasPresenter : IGetMetasOutputPort
    {
        public Dictionary<string, string> Result { get; private set; }
        public Task Success(Dictionary<string, string> metas)
        {
            Result = metas;
            return Task.CompletedTask;
        }
    }
}
