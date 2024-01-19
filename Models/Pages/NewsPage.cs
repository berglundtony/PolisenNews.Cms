using EPiServer.Filters;
using PolisenNews.Cms.Business;
using PolisenNews.Cms.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Pages
{
    /// <summary>
    /// Presents a news section including a list of the most recent articles on the site
    /// </summary>
    [ContentType(DisplayName = "News Page",
    GUID = "638D8271-5CA3-4C72-BABC-3E8779233263",
    Description = "Presents a news section including a list of the most recent articles on the site.",
    GroupName = SystemTabNames.Content)]
    [AvailableContentTypes(IncludeOn = new[] { typeof(PageData) })]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-12")]
    public class NewsPage : StandardPage
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 305)]
        public virtual PageListBlock NewsList { get; set; }
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            NewsList.Count = 20;
            NewsList.Heading = "";
            NewsList.IncludePublishDate = true;
            NewsList.PageTypeFilter = typeof(StandardPage).GetPageType();
            NewsList.SortOrder = FilterSortOrder.PublishedDescending;
        }
    }
}
