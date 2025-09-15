namespace Rubns.Core.Services
{
    public interface ILinkIdService
    {
        string Encode(int id);
        int Decode(string hash);
    }
}
