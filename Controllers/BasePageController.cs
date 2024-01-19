using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using PolisenNews.Cms.Models.Pages;
using PolisenNews.Cms.ViewModels;

namespace PolisenNews.Cms.Controllers
{
    public abstract class BasePageController<T> : PageController<T> where T : SitePageData
    {
        protected IActionResult PageView(T model)
        {
            var viewModel = new PageViewModel<T>(model);
            return PageView(viewModel);
        }

        protected IActionResult PageView(PageViewModel<T> viewModel)
        {
            viewModel.CurrentPage.MetaTitle ??= string.IsNullOrEmpty(viewModel.CurrentPage.MetaTitle) ? viewModel.CurrentPage.Name : viewModel.CurrentPage.MetaTitle;

            return View($"~/Views/Pages/{typeof(T).Name}.cshtml", viewModel);
        }
    }
}
