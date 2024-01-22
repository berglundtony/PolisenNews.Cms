using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using PolisenNews.Cms.Controllers;
using PolisenNews.Cms.Infrastructure;
using PolisenNews.Cms.Models.Blocks;
using PolisenNews.Cms.Models.Pages;

namespace PolisenNews.Cms.Business.Rendering
{
    [ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
    public class TemplateCoordinator : IViewTemplateModelRegistrator
    {
        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
            RegisterBlock<SectionMediaBlock>(viewTemplateModelRegistrator);
            RegisterBlock<PageListBlock>(viewTemplateModelRegistrator);
            RegisterBlock<ButtonBlock>(viewTemplateModelRegistrator);
        }


        private void RegisterPartial<T>(TemplateModelCollection viewTemplateModelRegistrator, string tagName) where T : SitePageData
        {
            viewTemplateModelRegistrator.Add(typeof(T), new EPiServer.DataAbstraction.TemplateModel
            {
                Name = $"{typeof(T).Name}-{tagName}",
                AvailableWithoutTag = false,
                Tags = new[] { tagName },
                Inherit = true,
                TemplateTypeCategory = EPiServer.Framework.Web.TemplateTypeCategories.MvcPartialView,
                Path = $"~/Views/{typeof(T).Name}/{tagName}.cshtml"
            });
        }
        //This is for blocks
        private void RegisterBlock<T>(TemplateModelCollection viewTemplateModelRegistrator) where T : BlockData
        {
            viewTemplateModelRegistrator.Add(typeof(T), new EPiServer.DataAbstraction.TemplateModel
            {
                Name = "SectionMediaBlock-Default",
                AvailableWithoutTag = true,
                TemplateTypeCategory = EPiServer.Framework.Web.TemplateTypeCategories.MvcPartialView,
                Path = $"~/Views/Shared/Components/{typeof(T).Name}.cshtml"
            });
        }
    }
}