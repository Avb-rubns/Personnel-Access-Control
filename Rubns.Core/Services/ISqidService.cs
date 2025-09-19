namespace Rubns.Core.Services
{
    public interface ISqidService
    {
        string Encode(int id);
        int Decode(string hash);



    }
}
