using EPiServer.SpecializedProperties;
using EPiServer.Web;
using PolisenNews.Cms.Business.Rendering;
using PolisenNews.Cms.Infrastructure;
using PolisenNews.Cms.Models.Blocks;
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
              GroupName = Globals.GroupNames.MetaData,
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
             GroupName = Globals.GroupNames.MetaData,
             Order = 110)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string MetaDescription { get; set; }

        [Display(
             GroupName = Globals.GroupNames.MetaData,
             Order = 120)]
        [CultureSpecific]
        [BackingType(typeof(PropertyStringList))]
        public virtual IList<string> MetaKeywords { get; set; }

        #endregion

        #region Settings

        [Display(
           GroupName = Globals.GroupNames.Settings,
           Order = 130)]
        [CultureSpecific]
        public virtual bool DisableIndexing { get; set; }

        [CultureSpecific]
            [Display(Name = "Hide site header", 
            GroupName = Globals.GroupNames.Settings, Order = 140)]
        public virtual bool HideSiteHeader { get; set; }

        [CultureSpecific]
            [Display(Name = "Hide site footer", 
            GroupName = Globals.GroupNames.Settings, Order = 150)]
        public virtual bool HideSiteFooter { get; set; }

        [Display(
            Name = "SiteLogotype",
            GroupName = Globals.GroupNames.Settings,
            Order = 160)]
        public virtual SiteLogotypeBlock SiteLogotype { get; set; }

        #endregion
        #region Teaser
        [Display(
            GroupName = Globals.GroupNames.Settings,
            Order = 170)]
        [CultureSpecific]
        public virtual bool DisplayImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
            [Display(Name = "Image", 
            GroupName = TabNames.Teaser, 
            Order = 180)]
        public virtual ContentReference PageImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Video)]
            [Display(Name = "Video", 
            GroupName = TabNames.Teaser, Order = 190)]
        public virtual ContentReference TeaserVideo { get; set; }

        [CultureSpecific]
        [Display(Name = "Teaser Header",
          GroupName = TabNames.Teaser,
          Order = 200)]
        public virtual string TeaserHeader { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
            [Display(Name = "Text", 
            GroupName = TabNames.Teaser, Order = 210)]
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
            [Display(Name = "Button label", 
            GroupName = TabNames.Teaser, 
            Order = 220)]
        public virtual string TeaserButtonText { get; set; }

        #endregion

        public string ContentAreaCssClass => "teaserblock";
    }
}

