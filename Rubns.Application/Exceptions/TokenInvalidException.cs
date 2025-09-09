namespace Rubns.Application.Exceptions
{
    public class TokenInvalidException : Exception
    {
        public string Title { get; }
        public TokenInvalidException(string? message, string title = "Token inválido o expirado") : base(message)
        {
            Title = title;
        }
    }
}
