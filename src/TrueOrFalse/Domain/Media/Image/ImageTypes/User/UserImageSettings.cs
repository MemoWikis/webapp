using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public class UserImageSettings : IImageSettings
{
    public  int Id { get; private set; }

    public IEnumerable<int> SizesSquare { get { return new[] { 512, 128, 85, 50, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 100, 500 }; } }

    public string BasePath { get { return "/Images/Users/"; } }

    public UserImageSettings(int id){
        Id = id;
    }

    public string ServerPathAndId(){
        return HttpContext.Current.Server.MapPath("/Images/Users/" + Id);
    }

    public ImageUrl GetUrl_128px(string emailAddress){ return GetUrl(emailAddress, 128, isSquare: true);}
    public ImageUrl GetUrl_85px_square(string emailAddress) { return GetUrl(emailAddress, 85, isSquare: true); }
    public ImageUrl GetUrl_50px(string emailAddress){ return GetUrl(emailAddress, 50);}

    private ImageUrl GetUrl(string emailAddress, int width, bool isSquare = false){
        return ImageUrl.Get(this, width, isSquare, arg => GetFallbackImage(emailAddress, arg));
    }

    protected string GetFallbackImage(string emailAddress, int width)
    {
        var sanitizedEmailAdress = emailAddress.Trim().ToLowerInvariant();
        var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(sanitizedEmailAdress));
        return "http://www.gravatar.com/avatar/" +
                BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant() + "?s=" + width + "&d=" +
                Uri.EscapeDataString(HttpContext.Current.Request.Url.Scheme + "://" +
                                     HttpContext.Current.Request.Url.Host +
                                     HttpContext.Current.Request.ApplicationPath + "Images/no-profile-picture-") + width + ".png";
    }
}

