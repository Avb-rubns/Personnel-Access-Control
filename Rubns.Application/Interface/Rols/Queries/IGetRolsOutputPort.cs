namespace Rubns.Application.Interface.Rols.Queries
{
    public interface IGetRolsOutputPort : IPresenter<List<RolDTO>>
    {
        Task Success(List<Rol> rols);
    }
}
