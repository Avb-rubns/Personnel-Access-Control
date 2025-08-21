namespace Rubns.Infrastructure.Services
{
    internal class RegexParserHtml : IHtmlParser
    {
        public List<Dictionary<string, string>> ParseHead(string html)
        {
            var result = new List<Dictionary<string, string>>();
            var tagRegex = new Regex(@"<(\w+)([^>]*)>(.*?)</\1>|<(\w+)([^>]*)/?>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (Match tagMatch in tagRegex.Matches(html))
            {
                var dict = new Dictionary<string, string>();

                string tagName = tagMatch.Groups[1].Success ? tagMatch.Groups[1].Value : tagMatch.Groups[4].Value;
                string attributes = tagMatch.Groups[2].Success ? tagMatch.Groups[2].Value : tagMatch.Groups[5].Value;
                string innerText = tagMatch.Groups[3].Value;

                dict["tag"] = tagName;

                // Capturar atributos
                var attrRegex = new Regex(@"(\w+)\s*=\s*[""']([^""']*)[""']", RegexOptions.IgnoreCase);
                foreach (Match attrMatch in attrRegex.Matches(attributes))
                {
                    dict[attrMatch.Groups[1].Value] = attrMatch.Groups[2].Value;
                }

                // Si tiene innerText, agregarlo
                if (!string.IsNullOrWhiteSpace(innerText))
                {
                    dict["content"] = innerText.Trim();
                }

                result.Add(dict);
            }

            return result;
        }

        public Dictionary<string, string> GetMeta(string html)
        {
            var metaTags = new Dictionary<string, string>();

            var titleMatch = Regex.Match(html, @"<title\b[^>]*>(.*?)</title>", RegexOptions.IgnoreCase);
            if (titleMatch.Success)
                metaTags["title"] = titleMatch.Groups[1].Value;

            var metaMatches = Regex.Matches(html, @"<meta\s+(?:name|property)=""(.*?)""\s+content=""(.*?)""", RegexOptions.IgnoreCase);
            foreach (Match match in metaMatches)
            {
                var key = match.Groups[1].Value;
                var value = match.Groups[2].Value;
                metaTags[key] = value;
            }

            return metaTags;
        }

        public string ExtractTitle(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            var match = Regex.Match(html, @"<title\b[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }
    }
}
