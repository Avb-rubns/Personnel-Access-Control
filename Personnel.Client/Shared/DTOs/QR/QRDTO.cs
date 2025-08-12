namespace Personnel.Client.Shared.DTOs.QR
{
    public class QRDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Status { get; set; }
        public DateTimeOffset Registered { get; set; }
        public string UserRegistered { get; set; }
        public DateTimeOffset LastModificated { get; set; }
        public string UserLastModificated { get; set; }

    }
}
