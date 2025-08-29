namespace Rubns.Core.Abstraccions
{
    public interface IPresenter<FormatType>
    {
        FormatType Content { get; }
    }
}
