using System.Collections.Generic;

public class QuestionImageSettings : ImageSettingsBase, IImageSettings
{
    public override int Id { get; set; }

    public IEnumerable<int> SizesSquare { get { return new[] { 512, 128, 50, 20 }; } }
    public IEnumerable<int> SizesFixedWidth { get { return new[] { 500, 435, 100 }; } }

    public override string BasePath { get { return "/Images/Questions/"; } }
    public string BaseDummyUrl { get { return "/Images/no-question-"; } }

    public QuestionImageSettings(){}

    public QuestionImageSettings(int questionId){
        Init(questionId);
    }

    public void Init(int questionId){
        Id = questionId;//$temp: wenn id = questionId, was ist dann bei mehreren Bildern
    }

    public ImageUrl GetUrl_128px_square() { return GetUrl(128, isSquare: true); }
    public ImageUrl GetUrl_128px() { return GetUrl(128); }
    public ImageUrl GetUrl_435px() { return GetUrl(435); }
    public ImageUrl GetUrl_500px() { return GetUrl(500); }

    private ImageUrl GetUrl(int width, bool isSquare = false){
        return ImageUrl.Get(this, width, isSquare, arg => BaseDummyUrl + width + ".png");
    }

    public static QuestionImageSettings Create(int questionId)
    {
        var result = new QuestionImageSettings();
        result.Init(questionId);
        return result;
    }
}