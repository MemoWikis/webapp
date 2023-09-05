using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class QuestionImageSettings : ImageSettings, IImageSettings
{
    private readonly QuestionReadingRepo _questionReadingRepo;
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.Question;
    public IEnumerable<int> SizesSquare => new[] { 512, 128, 50, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500, 435, 100 };

    public override string BasePath => Path.Combine(ImageFolderPath(), "Questions");
    public string BaseDummyUrl => Path.Combine(ImageFolderPath(), "no -question-");

    private Question? __question;
    private Question _question
    {
        get => __question ??= _questionReadingRepo.GetById(Id);
        set => __question = value;
    }

    public QuestionImageSettings(QuestionReadingRepo questionReadingRepo,
        IHttpContextAccessor httpContextAccessor, 
        IWebHostEnvironment webHostEnvironment) :base(httpContextAccessor, webHostEnvironment)
    {
        _questionReadingRepo = questionReadingRepo;
    }

    public QuestionImageSettings(int questionId,
        IHttpContextAccessor httpContextAccessor,
        IWebHostEnvironment webHostEnvironment) : base(httpContextAccessor, webHostEnvironment)
    {
        Init(questionId);
    }

    public void Init(int questionId){
        Id = questionId; //$temp: wenn id = questionId, was ist dann bei mehreren Bildern
    }

    public ImageUrl GetUrl_50px_square() => 
        GetUrl_WithCategoriesFailover(50, isSquare: true);

    private ImageUrl GetUrl_WithCategoriesFailover(int width, bool isSquare )
    {
        var imageUrl = GetUrl(50, isSquare: true);

        if (imageUrl.HasUploadedImage)
            return imageUrl;

        if (_question.Categories.Any())
            return new CategoryImageSettings(_question.Categories.First().Id,
                _contextAccessor, _webHostEnvironment).GetUrl(width, isSquare);

        return imageUrl;
    }

    public ImageUrl GetUrl_128px_square() { return GetUrl(128, isSquare: true); }
    public ImageUrl GetUrl_435px() { return GetUrl(435); }
    private ImageUrl GetUrl(int width, bool isSquare = false){
        return new ImageUrl(_contextAccessor, _webHostEnvironment)
            .Get(this, width, isSquare, arg => BaseDummyUrl + width + ".png");
    }
}