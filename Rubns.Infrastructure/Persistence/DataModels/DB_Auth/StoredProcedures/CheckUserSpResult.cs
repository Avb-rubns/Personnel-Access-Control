namespace Rubns.Infrastructure.Persistence.DataModels.DB_Auth.StoredProcedures
{
    public class CheckUserSpResult
    {
        public DateTime HourCheckIn { get; set; }
        public string Name { get; set; }
        public string Rol { get; set; }
        public string DistanceCheckIn { get; set; }
        public string AccessIn { get; set; }
        public DateTime HourCheckOut { get; set; }
        public string DistanceCheckOut { get; set; }
        public string AccessOut { get; set; }
    }
}
