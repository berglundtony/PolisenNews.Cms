using EPiServer.Security;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Infrastructure
{
    [GroupDefinitions]
    public static class TabNames
    {
        [Display(Order = 20)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Header = "Header";

        [Display(Order = 30)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string TopBanner = "Top Banner";

        [Display(Order = 40)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Footer = "Footer";

        [Display(Name = "Search settings", Order = 50)]
        public const string SearchSettings = "SearchSettings";

        [Display(Order = 60)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Menu = "Menu";

        [Display(Name = "Site labels", Order = 70)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteLabels = "SiteLabels";

        [Display(Name = "Site structure", Order = 80)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string SiteStructure = "SiteStructure";

        [Display(Order = 90)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Tags = "Tags";

        [Display(Order = 100)]
        public const string Location = "Location";

        [Display(Order = 110)]
        public const string Teaser = "Teaser";

        [Display(Name = "Custom settings", Order = 130)]
        public const string CustomSettings = "CustomSettings";

        [Display(Order = 200)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Styles = "Styles";

        [Display(Order = 210)]
        [RequiredAccess(AccessLevel.Edit)]
        public const string Scripts = "Scripts";

        [Display(Name = "Text", Order = 220)]
        public const string Text = "Text";

        [Display(Name = "Background", Order = 230)]
        public const string Background = "Background";

        [Display(Name = "Border", Order = 240)]
        public const string Border = "Border";

        [Display(Name = "Colors", Order = 245)]
        public const string Colors = "Colors";

        [Display(Name = "Image", Order = 250)]
        public const string Image = "Image";

        [Display(Name = "Block styling", Order = 260)]
        public const string BlockStyling = "BlockStyling";

        [Display(Name = "Button", Order = 270)]
        public const string Button = "Button";

        [Display(Name = "Specialized", Order = 280)]
        public const string Specialized = "Specialized";
    }
    /// <summary>
    /// Tags to use for the main widths used in the Bootstrap HTML framework
    /// </summary>
    public static class ContentAreaTags
    {
        public const string FullWidth = "full";
        public const string WideWidth = "wide";
        public const string HalfWidth = "half";
        public const string NarrowWidth = "narrow";
        public const string NoRenderer = "norenderer";
    }

    /// <summary>
    /// Names used for UIHint attributes to map specific rendering controls to page properties
    /// </summary>
    public static class SiteUIHints
    {
        public const string Contact = "contact";
        public const string Strings = "StringList";
        public const string StringsCollection = "StringsCollection";
    }
}
