using Microsoft.AspNetCore.Http;

public class ImageSettingsFactory
{
    public readonly QuestionReadingRepo _questionReadingRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageSettingsFactory(
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo)
    {
        _httpContextAccessor = httpContextAccessor;
        _questionReadingRepo = questionReadingRepo;
    }

    public T Create<T>(int typeId) where T : IImageSettings
    {
        if (typeof(T) == typeof(PageImageSettings))
        {
            return (T)(IImageSettings)new PageImageSettings(typeId, _httpContextAccessor);
        }
        if (typeof(T) == typeof(PageContentImageSettings))
        {
            return (T)(IImageSettings)new PageContentImageSettings(typeId, _httpContextAccessor);
        }

        if (typeof(T) == typeof(QuestionContentImageSettings))
        {
            return (T)(IImageSettings)new QuestionContentImageSettings(typeId, _httpContextAccessor);
        }

        if (typeof(T) == typeof(QuestionImageSettings))
        {
            return (T)(IImageSettings)new QuestionImageSettings(_questionReadingRepo,
                _httpContextAccessor);
        }

        if (typeof(T) == typeof(UserImageSettings))
        {
            return (T)(IImageSettings)new UserImageSettings(
                _httpContextAccessor);
        }

        throw new NotSupportedException($"Type {typeof(T).Name} not supported");
    }
}