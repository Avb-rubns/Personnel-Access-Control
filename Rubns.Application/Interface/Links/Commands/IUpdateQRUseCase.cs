namespace Rubns.Application.Interface.Links.Commands
{
    public interface IUpdateQRUseCase
    {
        Task ExecuteAsync(string id, JsonPatchDocument<QRUpdateDTO> qr);
    }
}
