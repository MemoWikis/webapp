using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public class ProfileImageSettings : IImageSettings
{
    private readonly int _profileId;

    public IEnumerable<int> SizesSquare{ get { return new[] { 512, 128, 50, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 100, 500 }; } }

    public string BasePath { get { return "/Images/Users/"; } }

    public ProfileImageSettings(int profileId){
        _profileId = profileId;
    }

    public string BasePathAndId(){
        return HttpContext.Current.Server.MapPath("/Images/Users/" + _profileId);
    }

    public ImageUrl GetUrl_128px(string emailAddress){
        return GetUrl(emailAddress, 128);
    }

    public ImageUrl GetUrl_50px(string emailAddress){
        return GetUrl(emailAddress, 50);
    }

    private ImageUrl GetUrl(string emailAddress, int width){
        return ImageUrl.Get(_profileId, width, BasePath, arg => GetFallbackImage(emailAddress,arg));
    }

    protected string GetFallbackImage(string emailAddress, int width)
    {
        var sanitizedEmailAdress = emailAddress.Trim().ToLowerInvariant();
        var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(sanitizedEmailAdress));
        return "http://www.gravatar.com/avatar/" +
                BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant() + "?s={0}&d=" +
                Uri.EscapeDataString(HttpContext.Current.Request.Url.Scheme + "://" +
                                     HttpContext.Current.Request.Url.Authority +
                                     HttpContext.Current.Request.ApplicationPath +
                                     "Images/no-profile-picture-") + width + ".png";
    }
}

