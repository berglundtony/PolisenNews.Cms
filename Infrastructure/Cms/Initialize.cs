using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using PolisenNews.Cms.Infrastructure.Cms.Settings;

namespace PolisenNews.Cms.Infrastructure.Cms
{
    [ModuleDependency(typeof(EPiServer.Shell.UI.InitializationModule))]//, typeof(SetupBootstrapRenderer))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            context.Services.AddTransient<IsInEditModeAccessor>(locator => () => locator.GetInstance<IContextModeResolver>().CurrentMode.EditOrPreview());
            context.Services.AddSingleton<ServiceAccessor<IContentRouteHelper>>(locator => locator.GetInstance<IContentRouteHelper>);
            context.Services.AddSingleton<ISettingsService, SettingsService>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {

        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}