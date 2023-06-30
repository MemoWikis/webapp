using System.Collections.Generic;
using System.Linq;

public class QuestionImageSettings : ImageSettings, IImageSettings
{
    private readonly QuestionRepo _questionRepo;
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.Question;
    public IEnumerable<int> SizesSquare => new[] { 512, 128, 50, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500, 435, 100 };

    public override string BasePath => "/Images/Questions/";
    public string BaseDummyUrl => "/Images/no-question-";

    private Question __question;
    private Question _question
    {
        get => __question ??= _questionRepo.GetById(Id);
        set => __question = value;
    }

    public QuestionImageSettings(QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
    }

    public QuestionImageSettings(Question question, QuestionRepo questionRepo)
    {
        _questionRepo = questionRepo;
        _question = question;

        Init(question.Id);
    }

    public QuestionImageSettings(int questionId){
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
            return new CategoryImageSettings(_question.Categories.First().Id).GetUrl(width, isSquare);

        return imageUrl;
    }

    public ImageUrl GetUrl_128px_square() { return GetUrl(128, isSquare: true); }
    public ImageUrl GetUrl_435px() { return GetUrl(435); }
    private ImageUrl GetUrl(int width, bool isSquare = false){
        return ImageUrl.Get(this, width, isSquare, arg => BaseDummyUrl + width + ".png");
    }

    public QuestionImageSettings Create(int questionId)
    {
        var result = new QuestionImageSettings(_questionRepo);
        result.Init(questionId);
        return result;
    }
}