
namespace Personnel.Client.Client.Pages.Admin
{
    public partial class User
    {
        RegisterUserDTO UserDTO { get; set; } = new();
        bool _processing = false;
    }
}
