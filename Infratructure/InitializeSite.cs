using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace PolisenNews.Cms.Infrastructure
{
    [ModuleDependency(typeof(Cms.Initialize))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]

    public class InitializeSite : IConfigurableModule
    {
        private IServiceCollection _services;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _services = context.Services;
            _services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            ServiceCollectionServiceExtensions.AddScoped(_services, x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }

        public void Initialize(InitializationEngine context)
        {

        }

        public void Uninitialize(InitializationEngine context)
        {

        }
    }
}
