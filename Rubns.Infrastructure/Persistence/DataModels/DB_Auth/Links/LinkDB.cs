namespace Rubns.Infrastructure.Persistence.DataModels.DB_Auth.Links
{
    public class LinkDb
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Status { get; set; }
        public DateTimeOffset Registered { get; set; }
        public int UserID { get; set; }
        public DateTimeOffset LastModificated { get; set; }
        public int LastUserID { get; set; }
        public QRDb QR { get; set; }
        public List<ClickDb> Clicks { get; set; }
    }
}
