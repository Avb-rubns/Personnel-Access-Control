
namespace Rubns.Core.DTOs.QR
{
    public class GenerateQrDTO
    {
        [Required(ErrorMessage = "Content is required.")]
        [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters.", MinimumLength = 10)]
        public string Content { get; set; }
        [Required(ErrorMessage = "Width in QR Code type is required.")]
        [Range(100, 1000, ErrorMessage = "Width must be between 100 and 1000 pixels.")]
        public int Width { get; set; } = 100;
        [Required(ErrorMessage = "Height in QR Code type is required.")]
        [Range(100, 1000, ErrorMessage = "Width must be between 100 and 1000 pixels.")]
        public int Height { get; set; } = 100;

    }
}
