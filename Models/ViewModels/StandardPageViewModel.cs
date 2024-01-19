using PolisenNews.Cms.Models.Pages;
using PolisenNews.Cms.ViewModels;

namespace PolisenNews.Cms.Models.ViewModels
{
    public class StandardPageViewModel : ContentViewModel<StandardPage>
    {
        public string CategoryName { get; set; }

        public StandardPageViewModel(StandardPage currentPage) : base(currentPage)
        {
        }

        public static StandardPageViewModel Create(StandardPage currentPage, CategoryRepository categoryRepository)
        {
            var model = new StandardPageViewModel(currentPage);
            if (currentPage.Category.Any())
            {
                model.CategoryName = categoryRepository.Get(currentPage.Category.FirstOrDefault()).Description;
            }
            return model;
        }
    }
}
