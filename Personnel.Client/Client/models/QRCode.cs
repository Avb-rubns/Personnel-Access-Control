namespace Personnel.Client.Client.models
{
    public class QRCode
    {
        public string Text { get; set; } = "https://github.com/Avb-rubns";
        public string ColorDark { get; set; } = "#000000";
        public string PI { get; set; } = "#000000";
        public string PO { get; set; } = "#000000";
        public string ColorLight { get; set; } = "#FFFFFF";

        public double DotScale { get; set; } = 1.0;
        public double DotScaleTiming { get; set; } = 1.0;
        public double DotScaleA { get; set; } = 1.0;

        public int QuietZone { get; set; } = 20;

        public string Logo { get; set; } = string.Empty;
    }
}
