using System.ComponentModel.DataAnnotations;

namespace Rubns.Core.DTOs.Users
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }
    }
}
