﻿using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VueApp;

public class ImageLicenseStoreController : BaseController
{
    private readonly ImageMetaDataReadingRepo _imageMetaDataReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageLicenseStoreController(SessionUser sessionUser,
        ImageMetaDataReadingRepo imageMetaDataReadingRepo , 
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment) : base(sessionUser)
    {
        _imageMetaDataReadingRepo = imageMetaDataReadingRepo;
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
    }
    [HttpGet]
    public JsonResult GetLicenseInfo(int id)
    {

        var imageFrontendData = new ImageFrontendData(_imageMetaDataReadingRepo
            .GetById(id), _httpContextAccessor, _webHostEnvironment);
        try
        {
            var imageUrl = imageFrontendData.GetImageUrl(1000, false, false, imageFrontendData.ImageMetaData.Type);

            if (imageFrontendData.ImageMetaDataExists && imageUrl.HasUploadedImage ||
                imageFrontendData.ImageMetaData != null && imageFrontendData.ImageMetaData.IsYoutubePreviewImage)
            {
                if (!imageFrontendData.ImageCanBeDisplayed)
                    return Json(new
                    {
                        imageCanBeDisplayed = false,
                        attributionHtmlString = imageFrontendData.AttributionHtmlString
                    });

                var dataIsYoutubeVideo = "";
                if (imageFrontendData.ImageMetaData.IsYoutubePreviewImage)
                    dataIsYoutubeVideo = $" data-is-youtube-video='{imageFrontendData.ImageMetaData.YoutubeKey}' ";

                var altDescription = String.IsNullOrEmpty(imageFrontendData.Description)
                    ? ""
                    : WebUtility.HtmlEncode(imageFrontendData.Description)
                        .Replace("\"", "'")
                        .Replace("„", "")
                        .Replace("“", "")
                        .Replace("{", "")
                        .Replace("}", "")
                        .StripHTMLTags()
                        .Truncate(120, true);

                return Json(new
                {
                    imageCanBeDisplayed = true,
                    url = imageUrl.Url,
                    alt = altDescription,
                    description = imageFrontendData.Description,
                    attributionHtmlString = imageFrontendData.AttributionHtmlString
                });
            }

            return Json(new
            {
                    imageCanBeDisplayed = false,
                    attributionHtmlString = imageFrontendData.AttributionHtmlString

            });
        }
        catch (Exception e)
        {
            new Logg(_httpContextAccessor, _webHostEnvironment).Error(e);
            return Json(new
            {
                imageCanBeDisplayed = false
            });
        }
    }

}