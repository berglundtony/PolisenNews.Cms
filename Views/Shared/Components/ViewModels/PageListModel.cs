using PolisenNews.Cms.Models.Blocks;
using PolisenNews.Cms.Models.Pages;

namespace PolisenNews.Cms.Views.Shared.Components.ViewModels
{
    public class PageListModel
    {
        public PageListModel(PageListBlock block)
        {
            Heading = block.Heading;
            ShowPublishDate = block.IncludePublishDate;
        }
        public string Heading { get; set; }

        public IEnumerable<PageData> Pages { get; set; }

        public bool ShowIntroduction { get; set; }

        public bool ShowPublishDate { get; set; }
    }
}
