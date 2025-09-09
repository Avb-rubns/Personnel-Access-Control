namespace Rubns.Infrastructure.Persistence.DataModels.DB_Auth.Rols
{
    public class RolDB
    {
        public int RolID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int LevelPermission { get; set; }
        public bool Status { get; set; }
    }
}
