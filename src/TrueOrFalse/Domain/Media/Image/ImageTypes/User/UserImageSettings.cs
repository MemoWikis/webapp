using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using static System.String;

public class UserImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }

    public ImageType ImageType => ImageType.User;

    public IEnumerable<int> SizesSquare => new[] { 512, 128, 85, 50, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 100, 500 };

    public override string BasePath => "/Images/Users/";
    public string BaseDummyUrl => "Images/no-profile-picture-";

    public UserImageSettings(int id){
        Id = id;
    }

    public void Init(int typeId)
    {
        throw new NotImplementedException();
    }

    public ImageUrl GetUrl_30px_square(User user) { return GetUrl(user, 30, isSquare: true); }
    public ImageUrl GetUrl_128px_square(User user) { return GetUrl(user, 128, isSquare: true);}
    public ImageUrl GetUrl_85px_square(User user) { return GetUrl(user, 85, isSquare: true); }
    public ImageUrl GetUrl_50px_square(User user) { return GetUrl(user, 50, isSquare: true); }
    public ImageUrl GetUrl_50px(User user) { return GetUrl(user, 50);}
    public ImageUrl GetUrl_200px(User user) { return GetUrl(user, 200); }
    public ImageUrl GetUrl_250px(User user) { return GetUrl(user, 250); }

    private ImageUrl GetUrl(User user, int width, bool isSquare = false){
        return ImageUrl.Get(this, width, isSquare, arg => GetFallbackImage(user, arg));
    }

    protected string GetFallbackImage(User user, int width)
    {
        var emailAddress = user.EmailAddress;

        if (user.IsFacebookUser())
        {
            var url = $"//graph.facebook.com/{user.FacebookId}/picture";

            if (width > 50)
                url += "?type=large";

            return url;
        }

        if (user.IsGoogleUser())
        {
            var url = $"https://www.googleapis.com/plus/v1/people/{user.GoogleId}?fields=image&key={Settings.GoogleApiKey}";

            try
            {
                var result = new WebClient().DownloadString(url);

                if (!IsNullOrEmpty(result) && result.Contains("url"))
                {
                    dynamic json = JsonConvert.DeserializeObject(result);
                    return ((string)json.image.url).Replace("sz=50", "sz=120");
                }
            }
            catch(WebException webException)
            {    
            }

        }

        var sanitizedEmailAdress = emailAddress.Trim().ToLowerInvariant();
        var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(sanitizedEmailAdress));
        return "//www.gravatar.com/avatar/" +
                BitConverter.ToString(hash).Replace("-", Empty).ToLowerInvariant() + "?s=" + width + "&d=" +
                Uri.EscapeDataString(HttpContext.Current.Request.Url.Scheme + "://" +
                                     HttpContext.Current.Request.Url.Host +
                                     HttpContext.Current.Request.ApplicationPath + BaseDummyUrl) + width + ".png";
    }


}

