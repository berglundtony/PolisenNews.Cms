using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using PolisenNews.Cms.Models;
using PolisenNews.Cms.Models.Pages;
using PolisenNews.Cms.Models.ViewModels;

namespace PolisenNews.Cms.Controllers
{
    public class StandardPageController : PageController<StandardPage>
    {
        private readonly CategoryRepository _categoryRepository;

        public StandardPageController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public ActionResult Index(StandardPage currentPage)
        {
            var model = StandardPageViewModel.Create(currentPage, _categoryRepository);
            return View(model);
        }
    }
}
