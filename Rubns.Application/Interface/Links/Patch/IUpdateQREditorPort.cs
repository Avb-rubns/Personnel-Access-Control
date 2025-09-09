namespace Rubns.Application.Interface.Links.Patch
{
    public interface IUpdateQREditorPort
    {
        Task UpdateQREditorPortAsync(int id, JsonPatchDocument<QRDTO> qr);
    }
}
