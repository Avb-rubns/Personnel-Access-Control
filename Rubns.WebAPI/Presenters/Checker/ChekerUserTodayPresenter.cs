using Rubns.Application.Interface.Checker.Queries;

namespace Rubns.WebAPI.Presenters.Checker
{
    internal class ChekerUserTodayPresenter : IGetCheckUserTodayOutputPort
    {
        public List<CheckUserTodayDTO> Result { get; set; }

        public Task Success(List<UserCheck> checks)
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

            Result = result;
            return Task.CompletedTask;
        }
    }
}
