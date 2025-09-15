namespace Rubns.Infrastructure.Persistence.DataModels.DB_Auth.Links
{
    public class QRDb
    {
        public int Id { get; set; }
        public int LinkId { get; set; }
        public double DotScale { get; set; }
        public string ColorDark { get; set; }
        public string ColorLight { get; set; }
        public int QuietZone { get; set; }
        public DateTimeOffset LastModificated { get; set; }
        public int LastUserID { get; set; }
        public LinkDb Link { get; set; }
    }
}
