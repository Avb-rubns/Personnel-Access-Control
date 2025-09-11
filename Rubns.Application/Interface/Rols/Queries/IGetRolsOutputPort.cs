namespace Rubns.Application.Interface.Rols.Queries
{
    public interface IGetRolsOutputPort : IPresenter<List<Rol>>
    {
        Task Success(List<Rol> rols);
    }
}
