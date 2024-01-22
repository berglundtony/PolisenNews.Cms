using EPiServer.Globalization;
using PolisenNews.Cms.Infrastructure;
using PolisenNews.Cms.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Pages
{
    [ContentType(DisplayName = "Home Page",
        GUID = "452d1812-7385-42c3-8073-c1b7481e7b20",
        Description = "Used for home page of all sites",
        AvailableInEditMode = true,
        GroupName = Globals.GroupNames.Content)]
    [ImageUrl("~/icons/cms/pages/CMS-icon-page-02.png")]
    public class HomePage : SitePageData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Title { get; set; }

        [Display(
            Name = "Main body",
            Description = "Main body",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [CultureSpecific]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            Name = "Content Area",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        [AllowedTypes(AllowedTypes = new[] { typeof(SectionMediaBlock), typeof(PageListBlock), typeof(ButtonBlock) })]
        public virtual ContentArea ContentArea { get; set; }
    }
}
