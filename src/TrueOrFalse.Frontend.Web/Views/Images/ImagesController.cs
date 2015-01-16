using System.Web.Mvc;
using TrueOrFalse;
using TrueOrFalse.Web.Context;

public class ImagesController : BaseController
{
    public string ImageDetailModal(int imgId)
    {
        var imageFrontendData = new ImageFrontendData(Resolve<ImageMetaDataRepository>().GetById(imgId));
        return ViewRenderer.RenderPartialView("ImageDetailModal", imageFrontendData, ControllerContext);
    }
}