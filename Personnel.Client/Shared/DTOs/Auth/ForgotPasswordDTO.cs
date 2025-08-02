namespace Personnel.Client.Shared.DTOs.Auth
{
    public class ForgotPasswordDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo valido")]
        public string Email { get; set; }
    }
}
