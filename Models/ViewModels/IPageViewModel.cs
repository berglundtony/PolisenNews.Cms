namespace PolisenNews.Cms.Models.ViewModels
{
    public interface IPageViewModel<out T> where T : PageData
    {
        T CurrentPage { get; }

        LayoutModel Layout { get; set; }

        IContent Section { get; set; }
    }
}
