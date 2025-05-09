using Microsoft.AspNetCore.Http;

public class QuestionImageSettings : ImageSettings, IImageSettings
{
    private readonly QuestionReadingRepo _questionReadingRepo;
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.Question;
    public IEnumerable<int> SizesSquare => new[] { 512, 128, 50, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500, 435, 100 };

    public override string BasePath => Settings.QuestionImageBasePath;
    public string BaseDummyUrl => "Placeholders/placeholder-question-";

    private Question? __question;
    private Question _question => __question ??= _questionReadingRepo.GetById(Id);

    public QuestionImageSettings(
        QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _questionReadingRepo = questionReadingRepo;
    }

    public QuestionImageSettings(
        int questionId,
        IHttpContextAccessor httpContextAccessor,
        QuestionReadingRepo questionReadingRepo) : base(httpContextAccessor)
    {
        Init(questionId);
        _questionReadingRepo = questionReadingRepo;
    }

    public void Init(int questionId)
    {
        Id = questionId;
    }

    public ImageUrl GetUrl_50px_square() =>
        GetUrl_WithPagesFailover(50, isSquare: true);

    private ImageUrl GetUrl_WithPagesFailover(int width, bool isSquare)
    {
        var imageUrl = GetUrl(50, isSquare: true);

        if (imageUrl.HasUploadedImage)
            return imageUrl;

        if (_question.Pages.Any())
            return new PageImageSettings(_question.Pages.First().Id,
                _contextAccessor).GetUrl(width, isSquare);

        return imageUrl;
    }

    public ImageUrl GetUrl_128px_square()
    {
        return GetUrl(128, isSquare: true);
    }

    public ImageUrl GetUrl_435px()
    {
        return GetUrl(435);
    }

    private ImageUrl GetUrl(int width, bool isSquare = false)
    {
        return new ImageUrl(_contextAccessor)
            .Get(this, width, isSquare, arg => BaseDummyUrl + width + ".png");
    }
}