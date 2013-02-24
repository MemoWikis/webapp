﻿using System;
using System.IO;
using System.Linq;
using System.Web;

public class ImageUrl
{
    public bool HasUploadedImage;
    public string Url;

    public static ImageUrl Get(
        int id, 
        int width,
        string basePath,
        Func<int, string> getFallBackImage)
    {
        var serverPath = HttpContext.Current.Server.MapPath(basePath);

        if(id != -1)
            if (Directory.GetFiles(serverPath, string.Format("{0}_*.jpg", id)).Any())
                return new ImageUrl { Url = basePath + id + "_" + width + ".jpg", HasUploadedImage = true };

        return new ImageUrl { Url = getFallBackImage(width), HasUploadedImage = false };
    }
}