namespace PolisenNews.Cms.Views.Shared.Components.ViewModels
{
    public class BlockViewModel<T> : IBlockViewModel<T> where T : BlockData
    {
        public BlockViewModel(T currentBlock) => CurrentBlock = currentBlock;

        public T CurrentBlock { get; }
    }
}
