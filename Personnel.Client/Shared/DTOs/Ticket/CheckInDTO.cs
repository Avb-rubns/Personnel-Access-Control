namespace Personnel.Client.Shared.DTOs.Ticket
{
    public class CheckInDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo valido")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo teléfono es obligatorio")]
        [MinLength(10, ErrorMessage = "El campo teléfono debe tener al menos 10 caracteres")]
        public string Phone { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string? IP { get; set; }
    }
}
