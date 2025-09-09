namespace Rubns.Application.Exceptions
{
    public class LoginException : Exception
    {
        public string Title { get; }
        public LoginException(string message, string title) : base(message)
        {
            Title = title;
        }
    }
}
