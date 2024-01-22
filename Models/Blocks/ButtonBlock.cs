using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Models.Blocks
{
    /// <summary>
    /// Used to insert a link which is styled as a button
    /// </summary>
    [ContentType(GUID = "426CF12F-1F01-4EA0-922F-0778314DDAF0")]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-26.png")]
    public class ButtonBlock :  BlockData
    {
        [Display(Order = 1, GroupName = SystemTabNames.Content)]
        [Required]
        public virtual string ButtonText { get; set; }

        [Display(Order = 2, GroupName = SystemTabNames.Content)]
        [Required]
        public virtual Url ButtonLink { get; set; }

    }
}
