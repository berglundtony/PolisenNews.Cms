using EPiServer.Filters;
using EPiServer.Web;
using PolisenNews.Cms.Infrastructure;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Blocks
{
    [ContentType(DisplayName = "Page List Block",
        GUID = "30685434-33DE-42AF-88A7-3126B936AEAD",
        Description = "A block that lists a bunch of pages",
        GroupName = Globals.GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-18.png")]
    public class PageListBlock : BlockData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [DefaultValue(true)]
        public virtual bool IncludePublishDate { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 4)]
        [Required]
        public virtual int Count { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 3)]
        [DefaultValue(FilterSortOrder.PublishedDescending)]
        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        public virtual FilterSortOrder SortOrder { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 5)]
        [Required]
        public virtual PageReference Root { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 6)]
        public virtual PageType PageTypeFilter { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 8)]
        public virtual bool Recursive { get; set; }

        [Display(
         GroupName = SystemTabNames.Content,
         Order = 9)]
        public virtual string Template { get; internal set; }

        [Display(
        GroupName = SystemTabNames.Content,
        Order = 10)]
        public bool IncludeTeaserText { get; internal set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Count = 5;
            IncludePublishDate = true;
            SortOrder = FilterSortOrder.PublishedDescending;
        }
    }
}