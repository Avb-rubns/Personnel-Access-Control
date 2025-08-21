namespace Rubns.Core.Services
{
    public interface IUtils
    {
        MultipartFormDataContent ToMultipartFormDataContent<T>(T obj);
        string CreateSlug(string s);
        string RemoveAccents(string text);
        string GenerateSlug(string text);
    }
}
