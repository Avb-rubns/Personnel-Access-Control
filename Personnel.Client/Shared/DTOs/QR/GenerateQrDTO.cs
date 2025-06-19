namespace Personnel.Client.Shared.DTOs.QR
{
    public class GenerateQrDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters.", MinimumLength = 10)]
        public string Content { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [Range(100, 1000, ErrorMessage = "Width must be between 100 and 1000 pixels.")]
        public int Width { get; set; } = 100;
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [Range(100, 1000, ErrorMessage = "Width must be between 100 and 1000 pixels.")]
        public int Height { get; set; } = 100;

    }
}
