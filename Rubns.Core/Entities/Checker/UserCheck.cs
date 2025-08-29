namespace Rubns.Core.Entities.Checker
{
    public class UserCheck
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
