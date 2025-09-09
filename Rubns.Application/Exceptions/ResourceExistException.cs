namespace Rubns.Application.Exceptions
{
    public class ResourceExistException : Exception
    {
        public string Title { get; }

        public ResourceExistException(string message, string title) : base(message)
        {
            Title = title;
        }

    }
}
