using Rubns.Application.Interface.Rols.Queries;

namespace Rubns.WebAPI.Presenters.Rols
{
    internal class RolsPresenter : IGetRolsOutputPort
    {
        public List<Rol> Result { get; private set; }

        public Task Success(List<Rol> rols)
        {
            Result = rols;
            return Task.CompletedTask;
        }
    }
}
