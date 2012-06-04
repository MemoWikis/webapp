using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Core;
using TrueOrFalse.Core.Web.Context;

public class UserProfileController : Controller
{
    private const string _viewLocation = "~/Views/User/UserProfile.aspx";

    private readonly UserRepository _userRepository;
    private readonly SessionUiData _sessionUiData;
    private readonly SessionUser _sessionUser;

    public UserProfileController(UserRepository userRepository, SessionUiData sessionUiData, SessionUser sessionUser)
    {
        _userRepository = userRepository;
        _sessionUiData = sessionUiData;
        _sessionUser = sessionUser;
    }

    public ViewResult Profile(string userName, int id)
    {
        var user = _userRepository.GetById(id);
        _sessionUiData.LastVisitedProfiles.Add(new UserNavigationModel(user));
        return View(_viewLocation, new UserProfileModel(user)
                                       {
                                           IsCurrentUserProfile = _sessionUser.User == user,
                                           ImageUrl = new GetUserImageUrl().Run(user)
                                       });
    }

    [HttpPost]
    public ViewResult UploadProfilePicture(HttpPostedFileBase file)
    {
        new StoreImages().Run(file.InputStream, Server.MapPath("/Images/Users/" + _sessionUser.User.Id));
        return Profile(_sessionUser.User.Name, _sessionUser.User.Id);
    }
}

public class GetUserImageUrl
{
    public string Run(User user)
    {
        const string relativePath = "/Images/Users/";
        var serverPath = HttpContext.Current.Server.MapPath(relativePath);
        if (Directory.GetFiles(serverPath, string.Format("{0}_*.jpg", user.Id)).Any())
        {
            return relativePath + user.Id + "_{0}.jpg";
        }
        else
        {
            return "/Images/no-profile-picture-{0}.png";
        }
    }
}

public class StoreImages
{
    public void Run(Stream inputStream, string basePath)
    {
        var sizes = new[] { 512, 128, 50, 20 };
        using (var image = Image.FromStream(inputStream))
        {
            foreach (var size in sizes)
            {
                using (var resized = new Bitmap(size, size))
                {
                    using (var graphics = Graphics.FromImage(resized))
                    {
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        if (image.Width > image.Height)
                        {
                            var scale = (float)size / image.Height;
                            graphics.DrawImage(image, -(image.Width * scale - size) / 2, 0, image.Width * scale, size);
                        }
                        else
                        {
                            var scale = (float)size / image.Width;
                            graphics.DrawImage(image, 0, -(image.Height * scale - size) / 2, size, image.Height * scale);
                        }
                    }
                    resized.Save(basePath + "_" + size + ".jpg", ImageFormat.Jpeg);
                }
            }
        }
    }
}
