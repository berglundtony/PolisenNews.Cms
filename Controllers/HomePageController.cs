using EPiServer.Web;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using PolisenNews.Cms.Models.Pages;
using PolisenNews.Cms.ViewModels;

namespace PolisenNews.Cms.Cms.Cms.Controllers
{
    public class HomePageController : ContentController<HomePage>
    {

        public IActionResult Index(HomePage currentContent)
        {
            if (string.IsNullOrEmpty(currentContent.Title))
            {
                currentContent = (HomePage)currentContent.CreateWritableClone();
            }
            var model = PageViewModel.Create(currentContent);

            if (SiteDefinition.Current.StartPage.CompareToIgnoreWorkID(currentContent.ContentLink))
            {
                // Connect the view models logotype property to the start page's to make it editable
                var editHints = ViewData.GetEditHints<PageViewModel<HomePage>, HomePage>();
                editHints.AddConnection(m => m.Layout.Logotype, p => p.SiteLogotype);
            }
            return View(model);
        }
            
    }
}
