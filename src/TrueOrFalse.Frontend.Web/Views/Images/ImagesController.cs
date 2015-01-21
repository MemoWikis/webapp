﻿using System;
using System.Web;
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

    public int GetQuestionImageId(string encodedPath)
    {
        var imagePath = HttpUtility.HtmlDecode(encodedPath);
        var splitArray = imagePath.ToLower()
            .Split(new string[] {"/images/questions/"}, StringSplitOptions.None);
        if (splitArray.Length < 2 || splitArray[1] == "")
        {
            return -1;
        }
        splitArray = splitArray[1].Split(new string[] { "_" }, StringSplitOptions.None);
        if (splitArray.Length < 2 || splitArray[0] == "")
        {
            return -1;
        }
        int typeId;
        if(Int32.TryParse(splitArray[0], out typeId))
            return Resolve<ImageMetaDataRepository>().GetBy(typeId, ImageType.Question).Id;

        return -1;
    }
}