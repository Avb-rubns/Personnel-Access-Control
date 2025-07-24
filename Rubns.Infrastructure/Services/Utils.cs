
namespace Rubns.Infrastructure.Services
{
    internal class Utils : IUtils
    {
        public MultipartFormDataContent ToMultipartFormDataContent<T>(T obj)
        {
            var formData = new MultipartFormDataContent();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(obj)?.ToString() ?? string.Empty;

                formData.Add(new StringContent(value), name.ToLower());
            }

            return formData;
        }
    }
}
