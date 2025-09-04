namespace Rubns.Application.Exceptions
{
    public class ResourceInactiveException : NotFoundException
    {
        public ResourceInactiveException(string message) : base(message)
        {
        }
    }
}
