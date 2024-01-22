using EPiServer.Framework.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PolisenNews.Cms.Models.Pages;
using PolisenNews.Cms.Models.ViewModels;
using PolisenNews.Cms.ViewModels;

namespace PolisenNews.Cms.Controllers
{
    /// <summary>
    /// Concrete controller that handles all page types that don't have their own specific controllers.
    /// </summary>
    /// <remarks>
    /// Note that as the view file name is hard coded it won't work with DisplayModes (ie Index.mobile.cshtml).
    /// For page types requiring such views add specific controllers for them. Alternatively the Index action
    /// could be modified to set ControllerContext.RouteData.Values["controller"] to type name of the currentPage
    /// argument. That may however have side effects.
    /// </remarks>
    [TemplateDescriptor(Inherited = true)]
    public class DefaultPageController : BasePageController<SitePageData>
    {
        public ViewResult Index(SitePageData currentPage)
        {
            IPageViewModel<SitePageData> model = CreateModel(currentPage);
            return View($"~/Views/Pages/{currentPage.GetOriginalType().Name}.cshtml", model);
        }

        /// <summary>
        /// Creates a PageViewModel where the type parameter is the type of the page.
        /// </summary>
        /// <remarks>
        /// Used to create models of a specific type without the calling method having to know that type.
        /// </remarks>
        private static IPageViewModel<SitePageData>? CreateModel(SitePageData currentPage)
        {
            var type = typeof(PageViewModel<>).MakeGenericType(currentPage.GetOriginalType());
            return Activator.CreateInstance(type, currentPage) as IPageViewModel<SitePageData>;
        }
    }
}
