using EPiServer.SpecializedProperties;

namespace PolisenNews.Cms.ViewModels
{
    public interface IFoundationContent
    {
        bool HideSiteHeader { get; set; }
        bool HideSiteFooter { get; set; }
        LinkItemCollection CssFiles { get; set; }
        string Css { get; set; }
    }
}
