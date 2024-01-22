using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PolisenNews.Cms.Infrastructure.Cms.Settings
{
    [SettingsContentType(DisplayName = "Scripts Injection Settings",
        GUID = "0156b963-88a9-450b-867c-e5c5e7be29fd",
        Description = "Scripts Injection Settings",
        SettingsName = "Scripts Injection")]
    public class ScriptInjectionSettings : SettingsBase
    {
        #region Scripts

        [JsonIgnore]
        [CultureSpecific]
        [Display(Name = "Header Scripts (Scripts will inject at the bottom of header)", GroupName = TabNames.Scripts, Description = "Scripts will inject at the bottom of header", Order = 10)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ScriptInjectionModel>))]
        public virtual IList<ScriptInjectionModel> HeaderScripts { get; set; }

        [JsonIgnore]
        [CultureSpecific]
        [Display(Name = "Footer Scripts (Scripts will inject at the bottom of footer)", GroupName = TabNames.Scripts, Description = "Scripts will inject at the bottom of footer", Order = 20)]
        [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ScriptInjectionModel>))]
        public virtual IList<ScriptInjectionModel> FooterScripts { get; set; }

        #endregion
    }

    public class ScriptInjectionModel
    {
        [UIHint(UIHint.Textarea)]
        [Display(Name = "External Scripts", Order = 30)]
        public virtual string ExternalScripts { get; set; }

        [UIHint(UIHint.Textarea)]
        [Display(Name = "Inline Scripts", Order = 40)]
        public virtual string InlineScripts { get; set; }
    }

    [PropertyDefinitionTypePlugIn]
    public class ScriptInjectionProperty : PropertyList<ScriptInjectionModel>
    {
    }
}
