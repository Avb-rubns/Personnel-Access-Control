namespace Personnel.Client.Shared.DTOs.Link.Queries
{
    public class LinkDTO
    {
        [Required]
        public string ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string Content { get; set; }
        public string Url { get; set; }
        public QRDTO QR { get; set; }
        public bool Status { get; set; }
        public DateTimeOffset Registered { get; set; }
        public int Clicks { get; set; }

    }
}
