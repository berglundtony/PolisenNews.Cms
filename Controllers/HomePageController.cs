using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using PolisenNews.Cms.Models.Pages;
using PolisenNews.Cms.ViewModels;

namespace PolisenNews.Cms.Cms.Cms.Controllers
{
    public class HomePageController : ContentController<HomePage>
    {
        private readonly IContentLoader contentLoader;

        public HomePageController(IContentLoader contentLoader)
        {
            this.contentLoader = contentLoader;
        }

        public IActionResult Index(HomePage currentContent)
        {
            if (string.IsNullOrEmpty(currentContent.Title))
            {
                currentContent = (HomePage)currentContent.CreateWritableClone();
            }
            var model = PageViewModel.Create(currentContent);

            return View(model);
        }
            
    }
}
