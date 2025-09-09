namespace Rubns.Application.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public string Title { get; }

        public UnauthorizedException(string message, string title) : base(message)
        {
            Title = title;

        }
    }
}
