namespace Rubns.Core.Ports.Link
{
    public interface IUpdateQREditorPort
    {
        Task<int> UpdateQREditorPortAsync(int id, JsonPatchDocument<QRDTO> qr);
    }
}
