using TrueOrFalse;

public abstract class QuestionEditData
{
    public string QuestionText;
    public string QuestionTextExtended;
    public LicenseQuestion ImageLicense;
    public QuestionVisibility Visibility;
    public string Solution;
    public string SolutionDescription;
    public SolutionType SolutionType;
    public string SolutionMetadataJson;

    public abstract string ToJson();

    //public abstract Category ToCategory(int categoryId);
}