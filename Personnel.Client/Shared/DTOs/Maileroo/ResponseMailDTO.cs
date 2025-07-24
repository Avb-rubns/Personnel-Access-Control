namespace Personnel.Client.Shared.DTOs.Maileroo
{
    public class ResponseMailDTO
    {
        public Data Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class Data
    {
        public string Reference_id { get; set; }
    }

}
