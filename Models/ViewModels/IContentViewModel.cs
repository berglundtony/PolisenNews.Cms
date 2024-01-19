using Microsoft.AspNetCore.Html;
using PolisenNews.Cms.Models.Pages;

namespace PolisenNews.Cms.Features.Shared
{
    public interface IContentViewModel<out TContent> where TContent : IContent
    {
        TContent CurrentContent { get; }
        HomePage StartPage { get; }
        HtmlString SchemaMarkup { get; }
    }
}
