using System.Text.RegularExpressions;

namespace Personnel.Client.Client.Pages.Forgot
{
    public partial class ResetPassword
    {
        [Inject] public NavigationManager Navigation { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        [Parameter] public string Token { get; set; } = string.Empty;

        ResetPasswordRequestDTO model = new();
        bool isValidToken = false;
        bool _loader = true;
        bool _send = false;
        bool _processing = false;
        public bool IsValid { get; set; }
        public bool HasMinimumLength { get; set; }
        public bool HasUppercaseLetter { get; set; }
        public bool HasLowercaseLetter { get; set; }
        public bool HasDigit { get; set; }
        public bool HasSpecialCharacter { get; set; }

        bool isShow;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private static readonly Regex minLengthRegex = new Regex(@".{8,}", RegexOptions.Compiled);
        private static readonly Regex uppercaseRegex = new Regex(@"[A-Z]", RegexOptions.Compiled);
        private static readonly Regex lowercaseRegex = new Regex(@"[a-z]", RegexOptions.Compiled);
        private static readonly Regex digitRegex = new Regex(@"[0-9]", RegexOptions.Compiled);
        private static readonly Regex specialCharRegex = new Regex(@"[!@#$%^&*]", RegexOptions.Compiled);



        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await Proxy.GetAsync<Response>($"api/v1/auth/reset-password/validate?token={Token}");

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        isValidToken = true;
                        model.ResetCode = Token;
                        break;
                    default:
                        isValidToken = false;
                        break;
                }
            }
            catch (Exception ex)
            {

            }

            _loader = false;
        }

        private async Task OnValidSubmit(EditContext context)
        {
            _processing = true;
            try
            {
                if (model.Password.Equals(model.NewPassword))
                {
                    var send = await Proxy.PutAsync<Response, ResetPasswordRequestDTO>("api/v1/auth/reset-password", model);

                    _send = send.StatusCode == HttpStatusCode.OK;
                }
                else
                {

                    Snackbar.Add("Las contraseñas no son iguales.", Severity.Error);
                }


            }
            catch (Exception ex) { }
            _processing = false;
        }

        void CheckingPassword(string password)
        {
            HasMinimumLength = minLengthRegex.IsMatch(password);
            HasUppercaseLetter = uppercaseRegex.IsMatch(password);
            HasLowercaseLetter = lowercaseRegex.IsMatch(password);
            HasDigit = digitRegex.IsMatch(password);
            HasSpecialCharacter = specialCharRegex.IsMatch(password);
        }

        void Visibility()
        {
            if (isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
    }
}
