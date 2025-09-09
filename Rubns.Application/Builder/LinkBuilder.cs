namespace Rubns.Application.Builder
{
    public class LinkBuilder
    {
        private string _name = string.Empty;
        private string _slug = string.Empty;
        private string _content = string.Empty;
        private string _url = string.Empty;
        private bool _status;
        private int _userId;
        private int? _id;

        public LinkBuilder WithId(int id) { _id = id; return this; }
        public LinkBuilder WithUserId(int userId) { _userId = userId; return this; }
        public LinkBuilder WithStatus(bool status) { _status = status; return this; }
        public LinkBuilder WithURL(string url) { _url = url; return this; }
        public LinkBuilder WithContent(string content) { _content = content; return this; }
        public LinkBuilder WithSlug(string slug) { _slug = slug; return this; }
        public LinkBuilder WithName(string name) { _name = name; return this; }

        public Link Build()
        {
            Link link = new Link();
            if (_id.HasValue) { link.ID = _id.Value; }
            link.Name = _name;
            link.Slug = _slug;
            link.Content = _content;
            link.Url = _url;
            link.Status = _status;
            link.UserIdRegistered = _userId;
            return link;
        }




    }
}
