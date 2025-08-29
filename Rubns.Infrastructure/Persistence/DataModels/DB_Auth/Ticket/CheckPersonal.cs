namespace Rubns.Infrastructure.Persistence.DataModels.DB_Auth.Ticket
{
    public class CheckPersonal
    {
        public int CheckPersonalID { get; set; }
        public int UserID { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? IP { get; set; }
        public DateTime Registed { get; set; }
    }
}
