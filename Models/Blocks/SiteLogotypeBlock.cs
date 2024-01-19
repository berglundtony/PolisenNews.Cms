using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Blocks
{
    /// <summary>
    /// Used to provide a composite property on the start page to set site logotype settings
    /// </summary>
    [ContentType(
        GUID = "09854019-91A5-4B93-8623-17F038346001",
        AvailableInEditMode = false)] // Should not be created and added to content areas by editors, the SiteLogotypeBlock is only used as a property type
    [ImageUrl("/images/pic02.jpg")]
    public class SiteLogotypeBlock : BlockData
    {
        /// <summary>
        /// Gets the site logotype URL
        /// </summary>
        /// <remarks>If not specified a default logotype will be used</remarks>
        [DefaultDragAndDropTarget]
        [UIHint(UIHint.Image)]
        public virtual Url Url
        {
            get
            {
                var url = this.GetPropertyValue(b => b.Url);

                return url == null || url.IsEmpty()
                    ? new Url("/gfx/logotype.png")
                    : url;
            }
            set => this.SetPropertyValue(b => b.Url, value);
        }

        [CultureSpecific]
        public virtual string Title { get; set; }
    }

}
