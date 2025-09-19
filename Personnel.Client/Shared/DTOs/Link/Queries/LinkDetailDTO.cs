namespace Personnel.Client.Shared.DTOs.Link.Queries
{
    public class LinkDetailDTO
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Status { get; set; }
        public DateTimeOffset Registered { get; set; }
        public string UserRegistered { get; set; }
        public DateTimeOffset LastModificated { get; set; }
        public string UserLastModificated { get; set; }
        public List<ClickDTO> Clicks { get; set; }
        public QRDTO QR { get; set; }
    }
}
