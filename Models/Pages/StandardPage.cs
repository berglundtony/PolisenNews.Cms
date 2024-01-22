using EPiServer.Web;
using PolisenNews.Cms.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Pages
{
    [ContentType(DisplayName = "StandardPage",
                GUID = "934E7266-FB8C-4DEA-B033-3B4E6AE6CBCF",
                Description = "The standard page.",
                GroupName = TabNames.Specialized,
                AvailableInEditMode = true)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-23.png")]
    public class StandardPage : SitePageData
    {
        [Display(
           GroupName = SystemTabNames.Content,
           Order = 50)]
        [CultureSpecific]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Main body",
            Description = "Main body",
            GroupName = SystemTabNames.Content,
            Order = 60)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
           Name = "Main Content Area",
           GroupName = SystemTabNames.Content,
           Order = 70)]
        public virtual ContentArea MainContentArea { get; set; }

    }
}
