using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Microsoft.AspNetCore.Html;
using PolisenNews.Cms.Features.Shared;
using PolisenNews.Cms.Infrastructure;
using PolisenNews.Cms.Models.Pages;

namespace PolisenNews.Cms.ViewModels
{
    public class ContentViewModel<TContent> : IContentViewModel<TContent> where TContent : IContent
    {
        private Injected<IContentLoader> _contentLoader;
        private Injected<IContentVersionRepository> _contentVersion;
        private Injected<IContextModeResolver> _contextModeResolver;
        private HomePage _startPage;

        public ContentViewModel() : this(default)
        {
        }

        public ContentViewModel(TContent currentContent)
        {
            CurrentContent = currentContent;
        }

        public TContent CurrentContent { get; set; }

        public virtual HomePage StartPage
        {
            get
            {
                if (_startPage == null)
                {
                    ContentReference currentStartPageLink = ContentReference.StartPage;
                    if (CurrentContent != null)
                    {
                        currentStartPageLink = CurrentContent.GetRelativeStartPage();
                    }

                    if (_contextModeResolver.Service.CurrentMode == ContextMode.Edit)
                    {
                        var startPageRef = _contentVersion.Service.LoadCommonDraft(currentStartPageLink, ContentLanguage.PreferredCulture.Name);
                        if (startPageRef == null)
                        {
                            _startPage = _contentLoader.Service.Get<HomePage>(currentStartPageLink);
                        }
                        else
                        {
                            _startPage = _contentLoader.Service.Get<HomePage>(startPageRef.ContentLink);
                        }
                    }
                    else
                    {
                        _startPage = _contentLoader.Service.Get<HomePage>(currentStartPageLink);
                    }
                }

                return _startPage;
            }
        }

        public HtmlString SchemaMarkup => throw new NotImplementedException();

    }
}
