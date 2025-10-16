using Microsoft.AspNetCore.Http;

public class ImageSettingsFactory(
    IHttpContextAccessor httpContextAccessor,
    QuestionReadingRepo questionReadingRepo)
{
    public readonly QuestionReadingRepo _questionReadingRepo = questionReadingRepo;

    public T Create<T>(int typeId) where T : IImageSettings
    {
        if (typeof(T) == typeof(PageImageSettings))
        {
            return (T)(IImageSettings)new PageImageSettings(typeId, httpContextAccessor);
        }

        if (typeof(T) == typeof(PageContentImageSettings))
        {
            return (T)(IImageSettings)new PageContentImageSettings(typeId, httpContextAccessor);
        }

        if (typeof(T) == typeof(QuestionContentImageSettings))
        {
            return (T)(IImageSettings)new QuestionContentImageSettings(typeId, httpContextAccessor);
        }

        if (typeof(T) == typeof(QuestionImageSettings))
        {
            return (T)(IImageSettings)new QuestionImageSettings(_questionReadingRepo,
                httpContextAccessor);
        }

        if (typeof(T) == typeof(UserImageSettings))
        {
            return (T)(IImageSettings)new UserImageSettings(
                httpContextAccessor);
        }

        throw new NotSupportedException($"Type {typeof(T).Name} not supported");
    }
}