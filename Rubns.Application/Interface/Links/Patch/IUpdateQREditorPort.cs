namespace Rubns.Application.Interface.Links.Patch
{
    public interface IUpdateQREditorPort
    {
        Task UpdateQRAsync(int id, JsonPatchDocument<QRDTO> qr);
    }
}
