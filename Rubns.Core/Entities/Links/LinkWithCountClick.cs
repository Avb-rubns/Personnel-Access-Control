namespace Rubns.Core.Entities.Links
{
    public class LinkWithCountClick
    {
        public int ID { get; set; }
        public string FriendlyId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Status { get; set; }
        public DateTimeOffset Registered { get; set; }
        public int UserID { get; set; }
        public DateTimeOffset LastModificated { get; set; }
        public int LastUserID { get; set; }
        public QR QR { get; set; }
        public int Clicks { get; set; }
    }
}
