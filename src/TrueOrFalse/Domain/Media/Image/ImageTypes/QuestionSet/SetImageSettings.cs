using System.Collections.Generic;
using static System.String;

public class SetImageSettings : ImageSettings, IImageSettings
{
    public override int Id { get; set; }
    public ImageType ImageType => ImageType.QuestionSet;
    public IEnumerable<int> SizesSquare => new[] { 206, 20 };
    public IEnumerable<int> SizesFixedWidth => new[] { 500 };
    public override string BasePath => "/Images/QuestionSets/";
    public string BaseDummyUrl => "/Images/no-set-";

    public SetImageSettings() {}

    public SetImageSettings(int setId){
        Init(setId);
    }

    public void Init(int setId){
        Id = setId;
    }
}