namespace Personnel.Client.Shared.DTOs.LogIn
{
    public class LoginRequestDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo valido")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo es obligatorio")]
        public string Password { get; set; }

    }
}
