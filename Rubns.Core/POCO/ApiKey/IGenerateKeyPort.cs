namespace Rubns.Core.POCO.ApiKey
{
    public interface IGenerateKeyPort<T>
    {
        T GenerateKey(RegisterDTO register, HttpRequest? request);
    }
}
