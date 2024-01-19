using PolisenNews.Cms.Infratructure.Cms.Settings;


internal static class ISettingsServiceExtensionsHelpers
{
    public static T GetSiteSettingsOrThrow<T>(this ISettingsService settingsService,
        Func<T, bool> shouldThrow,
        string message) where T : SettingsBase
    {
        var settings = settingsService.GetSiteSettings<T>();
        if (settings == null || (shouldThrow?.Invoke(settings) ?? false))
        {
            throw new InvalidOperationException(message);
        }

        return settings;
    }
}