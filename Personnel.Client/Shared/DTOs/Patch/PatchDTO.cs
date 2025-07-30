namespace Personnel.Client.Shared.DTOs.Patch
{
    public class PatchDTO
    {
        public string Op { get; set; }
        public string Path { get; set; }
        public string Value { get; set; }
    }
}
