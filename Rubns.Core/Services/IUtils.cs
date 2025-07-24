namespace Rubns.Core.Services
{
    public interface IUtils
    {
        MultipartFormDataContent ToMultipartFormDataContent<T>(T obj);
    }
}
