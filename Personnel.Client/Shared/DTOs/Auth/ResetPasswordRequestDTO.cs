namespace Personnel.Client.Shared.DTOs.Auth
{
    public class ResetPasswordRequestDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo es obligatorio.")]
        [RegularExpression
            (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,}).*$"
            , ErrorMessage = "la contraseña no cumple con los requisitos.")
        ]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ResetCode { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo correo es obligatorio.")]
        [RegularExpression
            (@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,}).*$"
            , ErrorMessage = "la contraseña no cumple con los requisitos.")
        ]
        public string NewPassword { get; set; }
    }
}
