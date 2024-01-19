using EPiServer.SpecializedProperties;
using EPiServer.Web;
using PolisenNews.Cms.Business.Rendering;
using PolisenNews.Cms.Infratructure;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Pages
{
    /// <summary>
    /// Base class for all page types
    /// </summary>
    public abstract class SitePageData : PageData, ICustomCssInContentArea
    {
        #region Metadata
        [Display(
              GroupName = TabNames.MetaData,
              Order = 100)]
        [CultureSpecific]
        public virtual string MetaTitle
        {
            get
            {
                var metaTitle = this.GetPropertyValue(p => p.MetaTitle);

                // Use explicitly set meta title, otherwise fall back to page name
                return !string.IsNullOrWhiteSpace(metaTitle)
                       ? metaTitle
                       : PageName;
            }
            set => this.SetPropertyValue(p => p.MetaTitle, value);
        }

        [Display(
             GroupName = TabNames.MetaData,
             Order = 200)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string MetaDescription { get; set; }

        [Display(
             GroupName = TabNames.MetaData,
             Order = 200)]
        [CultureSpecific]
        [BackingType(typeof(PropertyStringList))]
        public virtual IList<string> MetaKeywords { get; set; }

        #endregion

        #region Settings
        [Display(
           GroupName = TabNames.MetaData,
           Order = 400)]
        [CultureSpecific]
        public virtual bool DisableIndexing { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site header", GroupName = TabNames.Settings, Order = 200)]
        public virtual bool HideSiteHeader { get; set; }

        [CultureSpecific]
        [Display(Name = "Hide site footer", GroupName = TabNames.Settings, Order = 300)]
        public virtual bool HideSiteFooter { get; set; }

        #endregion
        #region Teaser

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Image", GroupName = TabNames.Teaser, Order = 100)]
        public virtual ContentReference PageImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Video)]
        [Display(Name = "Video", GroupName = TabNames.Teaser, Order = 200)]
        public virtual ContentReference TeaserVideo { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Name = "Text", GroupName = TabNames.Teaser, Order = 300)]
        public virtual string TeaserText
        {
            get
            {
                var teaserText = this.GetPropertyValue(p => p.TeaserText);

                // Use explicitly set teaser text, otherwise fall back to description
                return !string.IsNullOrWhiteSpace(teaserText)
                    ? teaserText
                    : MetaDescription;
            }
            set => this.SetPropertyValue(p => p.TeaserText, value);
        }

        [CultureSpecific]
        [Display(Name = "Button label", GroupName = TabNames.Teaser, Order = 600)]
        public virtual string TeaserButtonText { get; set; }


        public override void SetDefaultValues(ContentType contentType)
        {
            TeaserButtonText = "Read more";
            base.SetDefaultValues(contentType);
        }

        #endregion

        public string ContentAreaCssClass => "teaserblock";
    }
}

