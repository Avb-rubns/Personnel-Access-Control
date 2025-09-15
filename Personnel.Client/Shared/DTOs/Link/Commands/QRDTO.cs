namespace Personnel.Client.Shared.DTOs.Link.Commands
{
    public class QRDTO
    {
        public string Id { get; set; }
        public string LinkId { get; set; }
        public double DotScale { get; set; }
        public string ColorDark { get; set; }
        public string ColorLight { get; set; }
        public int QuietZone { get; set; }
        public DateTimeOffset LastModificated { get; set; }
        public string LastUserID { get; set; }
    }
}
