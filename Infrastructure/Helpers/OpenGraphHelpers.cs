using EPiServer.ServiceLocation;
using PolisenNews.Cms.Infrastructure.Cms.Settings;

namespace PolisenNews.Cms.Infrastructure.Helpers
{
    public static class OpenGraphHelpers
    {
        private static readonly Lazy<IContentLoader> _contentLoader = new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());
        private static readonly Lazy<IContentTypeRepository> _contentTypeRepository = new Lazy<IContentTypeRepository>(() => ServiceLocator.Current.GetInstance<IContentTypeRepository>());
        private static readonly Lazy<ISettingsService> _settingsService = new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());
        private static readonly Lazy<IContentLanguageAccessor> _cultureAccessor = new Lazy<IContentLanguageAccessor>(() => ServiceLocator.Current.GetInstance<IContentLanguageAccessor>());
    }
}
