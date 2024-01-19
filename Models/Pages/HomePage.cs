using PolisenNews.Cms.Infratructure;
using PolisenNews.Cms.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Pages
{
    [ContentType(DisplayName = "Home Page",
        GUID = "452d1812-7385-42c3-8073-c1b7481e7b20",
        Description = "Used for home page of all sites",
        AvailableInEditMode = true,
        GroupName = GroupNames.Content)]
    [ImageUrl("~/icons/cms/pages/CMS-icon-page-02.png")]
    public class HomePage : SitePageData
    {
        [Display(
              Name = "Title",
              GroupName = SystemTabNames.Content,
              Order = 1)]
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
        [AllowedTypes(AllowedTypes = new[] { typeof(SectionMediaBlock), typeof(PageListBlock) })]
        public virtual ContentArea ContentArea { get; set; }

        [Display(
            Name = "SiteLogotype",
            GroupName = TabNames.Settings,
            Order = 4)]
        public virtual SiteLogotypeBlock SiteLogotype { get; set; }
    }
}
