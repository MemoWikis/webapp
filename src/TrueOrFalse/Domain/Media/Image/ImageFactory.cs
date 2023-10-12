using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class ImageSettingsFactory(IHttpContextAccessor httpContextAccessor,
    IWebHostEnvironment webHostEnvironment,
    QuestionReadingRepo questionReadingRepo)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

    public T Create<T>(int typeId) where T : IImageSettings
    {
        if (typeof(T) == typeof(CategoryImageSettings))
        {
            return (T)(IImageSettings)new CategoryImageSettings(typeId, httpContextAccessor, webHostEnvironment);
        }

       if (typeof(T) == typeof(QuestionImageSettings))
        {
            return (T)(IImageSettings)new QuestionImageSettings(questionReadingRepo,
                httpContextAccessor, 
                webHostEnvironment);
        }

       if (typeof(T) == typeof(UserImageSettings))
       {
           return (T)(IImageSettings)new UserImageSettings(
               httpContextAccessor,
               webHostEnvironment);
       }


        throw new NotSupportedException($"Type {typeof(T).Name} not supported");
    }
}
