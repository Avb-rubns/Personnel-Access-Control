namespace Personnel.Client.Shared.DTOs.QR
{
    public class QRCreateDTO
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters and minimumLength 10 characters.", MinimumLength = 3)]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters and minimumLength 10 characters.", MinimumLength = 10)]
        public string Content { get; set; }
        public string Url { get; set; }
        public bool Status { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo {0} es obligatorio")]
        public int UserIDRegistered { get; set; }
    }
}
