namespace Rubns.WebAPI.Presenters.Checker
{
    internal class ChekerUserTodayPresenter : IGetCheckUserTodayOutPort
    {
        public List<CheckUserTodayDTO> Content { get; set; }

        public Task Handler(List<UserCheck> checks)
        {
            List<CheckUserTodayDTO> result = new List<CheckUserTodayDTO>();

            result = checks.Select(c => new CheckUserTodayDTO
            {
                Name = c.Name,
                Rol = c.Rol,
                AccessIn = c.AccessIn,
                DistanceCheckIn = c.DistanceCheckIn,
                HourCheckIn = c.HourCheckIn,
                AccessOut = c.AccessOut,
                DistanceCheckOut = c.DistanceCheckOut,
                HourCheckOut = c.HourCheckOut,
            }).ToList();

            Content = result;
            return Task.CompletedTask;
        }
    }
}
