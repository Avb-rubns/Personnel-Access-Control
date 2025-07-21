namespace Personnel.Client.Shared.DTOs.Users
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "El nombre debe de ser minimo de 3 caracteres.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "El apellido del usuario es obligatorio.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "El apellido debe de ser minimo de 3 caracteres.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El teléfono del usuario es obligatorio.")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "El teléfo debe de ser minimo de 10 caracteres.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Solo se permiten números.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "El usuario debe de tener un rol.")]
        public int RolID { get; set; }
        [Required(ErrorMessage = "El usuario debe de tener un estatus")]
        public bool Status { get; set; }

    }
}
