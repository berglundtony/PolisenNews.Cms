namespace PolisenNews.Cms.Views.Shared.Components.ViewModels
{
    public interface IBlockViewModel<out T> where T : BlockData
    {
        T CurrentBlock { get; }
    }
}
