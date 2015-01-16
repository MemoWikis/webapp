using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using TrueOrFalse;

public class UserImageSettings : ImageSettingsBase, IImageSettings
{
    public override int Id { get; set; }

    public ImageType ImageType
    {
        get { return ImageType.User; }
    }

    public IEnumerable<int> SizesSquare { get { return new[] { 512, 128, 85, 50, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 100, 500 }; } }

    public override string BasePath { get { return "/Images/Users/"; } }
    public string BaseDummyUrl { get { return "Images/no-profile-picture-"; } }

    public UserImageSettings(int id){
        Id = id;
    }

    public void Init(int typeId)
    {
        throw new NotImplementedException();
    }

    public ImageUrl GetUrl_20px_square(string emailAddress){return GetUrl(emailAddress, 20, isSquare: true);}
    public ImageUrl GetUrl_30px_square(string emailAddress) { return GetUrl(emailAddress, 30, isSquare: true); }
    public ImageUrl GetUrl_128px_square(string emailAddress){ return GetUrl(emailAddress, 128, isSquare: true);}
    public ImageUrl GetUrl_85px_square(string emailAddress) { return GetUrl(emailAddress, 85, isSquare: true); }
    public ImageUrl GetUrl_50px(string emailAddress){ return GetUrl(emailAddress, 50);}
    public ImageUrl GetUrl_200px(string emailAddress) { return GetUrl(emailAddress, 200); }
    public ImageUrl GetUrl_250px(string emailAddress) { return GetUrl(emailAddress, 250); }

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
                                     HttpContext.Current.Request.ApplicationPath + BaseDummyUrl) + width + ".png";
    }


}

