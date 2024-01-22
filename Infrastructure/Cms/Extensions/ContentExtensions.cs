using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;

namespace PolisenNews.Cms.Infrastructure.Cms.Extensions
{
    public static class ContentExtensions
    {
        private static readonly Lazy<IContentLoader> _contentLoader = new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());
        private const string Delimiter = "^!!^";

        public static IEnumerable<PageData> GetSiblings(this PageData pageData) => GetSiblings(pageData, _contentLoader.Value);

        public static IEnumerable<PageData> GetSiblings(this PageData pageData, IContentLoader contentLoader)
        {
            var filter = new FilterContentForVisitor();
            return contentLoader.GetChildren<PageData>(pageData.ParentLink).Where(page => !filter.ShouldFilter(page));
        }

        public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> contents, bool requirePageTemplate = false,
            bool requireVisibleInMenu = false)
            where T : IContent
        {
            var accessFilter = new FilterAccess();
            var publishedFilter = new FilterPublished();
            contents = contents.Where(x => !publishedFilter.ShouldFilter(x) && !accessFilter.ShouldFilter(x));
            if (requirePageTemplate)
            {
                var templateFilter = ServiceLocator.Current.GetInstance<FilterTemplate>();
                templateFilter.TemplateTypeCategories = TemplateTypeCategories.Request;
                contents = contents.Where(x => !templateFilter.ShouldFilter(x));
            }

            if (requireVisibleInMenu)
            {
                contents = contents.Where(x => VisibleInMenu(x));
            }

            return contents;
        }

        private static bool VisibleInMenu(IContent content)
        {
            var page = content as PageData;
            return page == null || page.VisibleInMenu;
        }
    }
}
