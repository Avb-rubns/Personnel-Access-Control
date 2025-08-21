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

        public string CreateSlug(string s)
        {
            string slug = string.Empty;

            slug = s.Trim().ToLower();
            slug = RemoveAccents(slug);

            slug = Regex.Replace(s, @"\s", "-");

            return slug;
        }
        public string RemoveAccents(string text)
        {

            string normalizedString = text.Normalize(NormalizationForm.FormD);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {

                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public string GenerateSlug(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            // Convertir a minúsculas
            text = text.ToLowerInvariant();

            // Eliminar acentos
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            text = sb.ToString().Normalize(NormalizationForm.FormC);

            // Reemplazar espacios y caracteres no válidos
            text = Regex.Replace(text, @"\s+", "-");
            text = Regex.Replace(text, @"[^a-z0-9\-]", "");
            text = Regex.Replace(text, @"-+", "-");
            text = text.Trim('-');

            return text;
        }

    }
}
