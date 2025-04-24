using Microsoft.AspNetCore.Http;

public class UserImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }

    public ImageType ImageType => ImageType.User;

    public override string BasePath => Settings.UserImageBasePath;
    public string BaseDummyUrl => "no-profile-picture-";

    public UserImageSettings(
        int id,
        IHttpContextAccessor httpContextAccessor) :
        base(httpContextAccessor)
    {
        Id = id;
    }

    public UserImageSettings(IHttpContextAccessor httpContextAccessor) :
        base(httpContextAccessor)
    {
    }

    public void Init(int typeId)
    {
        Id = typeId;
    }

    public IEnumerable<int> SizesSquare => new[] { 512, 256, 128, 85, 50, 20 };

    public ImageUrl GetUrl_512px_square(IUserTinyModel user)
    {
        return GetUrl(user, 512, isSquare: true);
    }

    public ImageUrl GetUrl_256px_square(IUserTinyModel user)
    {
        return GetUrl(user, 256, isSquare: true);
    }

    public ImageUrl GetUrl_128px_square(IUserTinyModel user)
    {
        return GetUrl(user, 128, isSquare: true);
    }

    public ImageUrl GetUrl_85px_square(IUserTinyModel user)
    {
        return GetUrl(user, 85, isSquare: true);
    }

    public ImageUrl GetUrl_50px_square(IUserTinyModel user)
    {
        return GetUrl(user, 50, isSquare: true);
    }

    public ImageUrl GetUrl_20px_square(IUserTinyModel user)
    {
        return GetUrl(user, 20, isSquare: true);
    }

    public IEnumerable<int> SizesFixedWidth => new[] { 100, 250, 500 };

    public ImageUrl GetUrl_500px(IUserTinyModel user)
    {
        return GetUrl(user, 500);
    }

    public ImageUrl GetUrl_250px(IUserTinyModel user)
    {
        return GetUrl(user, 250);
    }

    public ImageUrl GetUrl_50px(IUserTinyModel user)
    {
        return GetUrl(user, 50);
    }

    private ImageUrl GetUrl(IUserTinyModel user, int width, bool isSquare = false)
    {
        return new ImageUrl(_contextAccessor)
            .Get(this, width, isSquare, arg => GetFallbackImage(user, arg));
    }

    public IEnumerable<int> SizesFallback => new[] { 533, 350, 250, 128, 85, 50, 20 };

    protected string GetFallbackImage(IUserTinyModel user, int width)
    {
        //Removed Google, Facebook and Gravatar urls for the time being, to be reintroduced with images fetched server side

        //fallback images need to be updated
        if (width == 512)
            width = 533;
        else if (width == 256)
            width = 350;

        return Path.Combine(BaseDummyUrl + width + ".png");
    }
}