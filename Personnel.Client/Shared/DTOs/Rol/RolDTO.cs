namespace Personnel.Client.Shared.DTOs.Rol
{
    public class RolDTO
    {
        public int RolID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int LevelPermission { get; set; }

        public bool Status { get; set; }

    }
}
