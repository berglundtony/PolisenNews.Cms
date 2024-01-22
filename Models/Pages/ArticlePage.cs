using System.ComponentModel.DataAnnotations;
namespace PolisenNews.Cms.Models.Pages
{
    /// <summary>
    /// Used primarily for publishing news articles on the website
    /// </summary>

    [ContentType(DisplayName = "ArticlePage",
        GUID = "AEECADF2-3E89-4117-ADEB-F8D43565D2F4",
        Description = "Used for articles of all sites",
        AvailableInEditMode = true,
        GroupName = SystemTabNames.Content)]
    [AvailableContentTypes(IncludeOn = new[] { typeof(PageData) })]
    [ImageUrl("~/icons/cms/pages/CMS-icon-page-07.png")]
    public class ArticlePage : SitePageData
    {
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            VisibleInMenu = false;
        }

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
