namespace Rubns.Infrastructure.Services
{
    public class SqidService
        : ISqidService
    {
        private readonly SqidsEncoder<int> _sqids;
        private readonly IConfiguration _configuration;

        public SqidService(IConfiguration configuration)
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
            int salt = new Random().Next(0, int.MaxValue);
            return _sqids.Encode(id, salt);
        }
    }
}
