using Microsoft.AspNetCore.Http;

public class QuestionContentImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.QuestionContent;
    public IEnumerable<int> SizesSquare => [128];
    public IEnumerable<int> SizesFixedWidth => [800];

    public override string BasePath => Settings.QuestionContentImageBasePath;
    public string BaseDummyUrl => "Placeholders/placeholder-question-128.png";

    public QuestionContentImageSettings(
        int questionId,
        IHttpContextAccessor contextAccessor) :
        base(contextAccessor)
    {
        Id = questionId;
    }

    public void Init(int questionId)
    {
        Id = questionId;
    }
}