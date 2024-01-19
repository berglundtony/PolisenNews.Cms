using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Blocks
{
    [ContentType(
        DisplayName = "Section Media Block",
        GUID = "EB67A99A-E239-41B8-9C59-20EAA5936047")]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-07.png")]
    public class SectionMediaBlock: BlockData
    {
        [Display(
           Name = "Headline",
           GroupName = SystemTabNames.Content,
           Order = 10)]
        public virtual string Headline { get; set; }

        [Display(
             Name = "Body",
             GroupName = SystemTabNames.Content,
             Order = 20)]
        public virtual XhtmlString Body { get; set; }

        [Display(
             Name = "Image",
             GroupName = SystemTabNames.Content,
             Order = 30)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }
    }
}
