namespace Rubns.WebAPI.Presenters.Rols
{
    internal class RolsPresenter : IGetRolsOutPort
    {
        public List<Rol> Content { get; private set; }

        public Task Handler(List<Rol> rols)
        {
            Content = rols;
            return Task.CompletedTask;
        }
    }
}
