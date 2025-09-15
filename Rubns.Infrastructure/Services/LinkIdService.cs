namespace Rubns.Infrastructure.Services
{
    public class LinkIdService
        : ILinkIdService
    {
        private readonly SqidsEncoder<int> _sqids;
        private readonly IConfiguration _configuration;

        public LinkIdService(IConfiguration configuration)
        {
            _configuration = configuration;
            string alpha = _configuration.GetSection("AlphabetSqid").Get<string>();
            _sqids = new SqidsEncoder<int>(new SqidsOptions() { MinLength = 10, Alphabet = alpha });
        }

        public int Decode(string hash)
        {

            var numbers = _sqids.Decode(hash);
            return numbers.Count > 0 ? numbers[0] : 0;
        }

        public string Encode(int id)
        {
            return _sqids.Encode(id);
        }
    }
}
