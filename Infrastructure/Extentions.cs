using EPiServer.ServiceLocation;
using PolisenNews.Cms.Models.Pages;

namespace PolisenNews.Cms.Infrastructure
{
    public static class Extentions
    {
        private static readonly Lazy<IContentRepository> _contentRepository =
            new Lazy<IContentRepository>(() => ServiceLocator.Current.GetInstance<IContentRepository>());

        public static ContentReference GetRelativeStartPage(this IContent content)
        {
            if (content is HomePage)
            {
                return content.ContentLink;
            }

            var ancestors = _contentRepository.Value.GetAncestors(content.ContentLink);
            var startPage = ancestors.FirstOrDefault(x => x is HomePage) as HomePage;
            return startPage == null ? ContentReference.StartPage : startPage.ContentLink;
        }
    }
}
