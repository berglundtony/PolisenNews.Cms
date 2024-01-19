using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PolisenNews.Cms.Business.Rendering;

namespace PolisenNews.Cms.Views
{
    public abstract class PageBase<TModel> : RazorPage<TModel> where TModel : class
    {
        private readonly ContentAreaItemRenderer _contentAreaItemRenderer;

        public abstract override Task ExecuteAsync();

        public PageBase() : this(ServiceLocator.Current.GetInstance<ContentAreaItemRenderer>())
        {
        }

        public PageBase(ContentAreaItemRenderer contentAreaItemRenderer)
        {
            _contentAreaItemRenderer = contentAreaItemRenderer;
        }

        protected void OnItemRendered(ContentAreaItem contentAreaItem, TagHelperContext context, TagHelperOutput output)
        {
            _contentAreaItemRenderer.RenderContentAreaItemCss(contentAreaItem, context, output);
        }
    }
}
