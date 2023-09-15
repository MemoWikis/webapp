using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


public class UserImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }

    public ImageType ImageType => ImageType.User;

    public IEnumerable<int> SizesSquare => new[] { 512, 128, 85, 50, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 100, 500 };

    public override string BasePath => "Images/Users";
    public string BaseDummyUrl => "Images/no-profile-picture-";

    public UserImageSettings(int id,
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment):
        base(httpContextAccessor, webHostEnvironment){
        Id = id;
    }

    public UserImageSettings(IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) :
        base(httpContextAccessor, webHostEnvironment) {}

    public void Init(int typeId)
    {
        throw new NotImplementedException();
    }

    public ImageUrl GetUrl_128px_square(IUserTinyModel user) { return GetUrl(user, 128, isSquare: true);}
    public ImageUrl GetUrl_85px_square(IUserTinyModel user) { return GetUrl(user, 85, isSquare: true); }
    public ImageUrl GetUrl_50px_square(IUserTinyModel user) { return GetUrl(user, 50, isSquare: true); }
    public ImageUrl GetUrl_50px(IUserTinyModel user) { return GetUrl(user, 50);}
    public ImageUrl GetUrl_250px(IUserTinyModel user) { return GetUrl(user, 250); }
    public ImageUrl GetUrl_20px(IUserTinyModel user) { return GetUrl(user, 20); }

    private ImageUrl GetUrl(IUserTinyModel user, int width, bool isSquare = false) {
        return new ImageUrl(_contextAccessor, _webHostEnvironment)
            .Get(this, width, isSquare, arg => GetFallbackImage(user, arg));
    }

    protected string GetFallbackImage(IUserTinyModel user, int width)
    {
        //Removed Google, Facebook and Gravatar urls for the time being, to be reintroduced with images fetched server side
        return Path.Combine( "/Images/" + BaseDummyUrl + width + ".png");
    }
}

