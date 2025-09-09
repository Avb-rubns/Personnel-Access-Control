namespace Rubns.Application.Interface.Rols.Get
{
    public interface IGetRolsOutPort : IPresenter<List<Rol>>
    {
        Task Handler(List<Rol> rols);
    }
}
