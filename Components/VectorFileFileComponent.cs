using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using PolisenNews.Cms.Models.Medias;

namespace PolisenNews.Cms.Components
{
    public class VectorFileComponent : AsyncPartialContentComponent<VectorImageMediaData>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(VectorImageMediaData currentBlock)
        {
            return await Task.FromResult(View("~/Views/Media/Components/VectorFile/.Default.cshtml", currentBlock));
        }
    }
}
