using Personnel.Client.Shared.DTOs.Link.Commands;

namespace Rubns.Application.Interface.Links.Commands
{
    public interface IUpdateQRUseCase
    {
        Task ExecuteAsync(int id, JsonPatchDocument<QRDTO> qr);
    }
}
