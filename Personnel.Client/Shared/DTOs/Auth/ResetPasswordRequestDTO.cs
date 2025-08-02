namespace Personnel.Client.Shared.DTOs.Auth
{
    public class ResetPasswordRequestDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo valido")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ResetCode { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string NewPassword { get; set; }
    }
}
