﻿using EPiServer.Filters;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using PolisenNews.Cms.Business;
using PolisenNews.Cms.Models.Blocks;
using PolisenNews.Cms.Models.Pages;
using PolisenNews.Cms.Views.Shared.Components.ViewModels;

namespace PolisenNews.Cms.Components
{
    public class PageListBlockViewComponent : BlockComponent<PageListBlock>
    {
        private readonly ContentLocator _contentLocator;
        private readonly IContentLoader _contentLoader;

        public PageListBlockViewComponent(ContentLocator contentLocator, IContentLoader contentLoader)
        {
            _contentLocator = contentLocator;
            _contentLoader = contentLoader;
        }

        protected override IViewComponentResult InvokeComponent(PageListBlock currentContent)
        {
            var pages = FindPages(currentContent);

            pages = Sort(pages, currentContent.SortOrder);

            if (currentContent.Count > 0)
            {
                pages = pages.Take(currentContent.Count);
            }

            var model = new PageListModel(currentContent)
            {
                Pages = pages.Cast<PageData>()
            };

            ViewData.GetEditHints<PageListModel, PageListBlock>()
                .AddConnection(x => x.Heading, x => x.Heading);

            return View("~/Views/Shared/Components/PageListBlock.cshtml", model);
        }

        private IEnumerable<PageData> FindPages(PageListBlock currentBlock)
        {
            IEnumerable<PageData> pages;
            var listRoot = currentBlock.Root;

            pages = _contentLocator.GetAll<PageData>(listRoot);

            return pages;
        }

        private static IEnumerable<PageData> Sort(IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(new PageDataCollection(pages.ToList()));
            return pages;
        }
    }
}