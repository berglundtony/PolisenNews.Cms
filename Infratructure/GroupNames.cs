using System.ComponentModel.DataAnnotations;

namespace PolisenNews.Cms.Infratructure
{
    [GroupDefinitions]
    public static class GroupNames
    {
        [Display(Name = "Content", Order = 510)]
        public const string Content = "Content";

        [Display(Order = 520)]
        public const string Forms = "Forms";

        [Display(Order = 530)]
        public const string LocationBlocks = "Location Blocks";

        [Display(Order = 540)]
        public const string Multimedia = "Multimedia";

    }
}
