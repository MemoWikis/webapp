using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using TrueOrFalse;

public class GetUserImageUrl : GetImageUrl<User>
{

    protected override string GetFallbackImage(User user)
    {
        var sanitizedEmailAdress = user.EmailAddress.Trim().ToLowerInvariant();
        var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(sanitizedEmailAdress));
        return "http://www.gravatar.com/avatar/" + 
                BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant() + "?s={0}&d=" +
                Uri.EscapeDataString(HttpContext.Current.Request.Url.Scheme + "://" + 
                                     HttpContext.Current.Request.Url.Authority + 
                                     HttpContext.Current.Request.ApplicationPath + 
                                     "Images/no-profile-picture-") + "{0}.png";
    }

    protected override string RelativePath
    {
        get {  return "/Images/Users/"; }
    }
}
