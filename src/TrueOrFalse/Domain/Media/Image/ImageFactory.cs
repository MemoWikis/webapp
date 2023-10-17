using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class ImageSettingsFactory
{
    public readonly QuestionReadingRepo _questionReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImageSettingsFactory(IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment,
        QuestionReadingRepo questionReadingRepo)
    {
        _httpContextAccessor = httpContextAccessor;
        _webHostEnvironment = webHostEnvironment;
        _questionReadingRepo = questionReadingRepo;
    }

    public T Create<T>(int typeId) where T : IImageSettings
    {
        if (typeof(T) == typeof(CategoryImageSettings))
        {
            return (T)(IImageSettings)new CategoryImageSettings(typeId, _httpContextAccessor, _webHostEnvironment);
        }

       if (typeof(T) == typeof(QuestionImageSettings))
        {
            return (T)(IImageSettings)new QuestionImageSettings(_questionReadingRepo,
                _httpContextAccessor, 
                _webHostEnvironment);
        }

       if (typeof(T) == typeof(UserImageSettings))
       {
           return (T)(IImageSettings)new UserImageSettings(
               _httpContextAccessor,
               _webHostEnvironment);
       }


        throw new NotSupportedException($"Type {typeof(T).Name} not supported");
    }
}
