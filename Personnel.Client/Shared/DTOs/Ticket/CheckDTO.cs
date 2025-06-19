namespace Personnel.Client.Shared.DTOs.Ticket
{
    public class CheckDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo es obligatorio")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo teléfono es obligatorio")]
        public string Phone { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
