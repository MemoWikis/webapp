public abstract class QuestionEditData
{
    public string QuestionText;
    public string TextHtml;
    public string QuestionTextExtended;
    public string TextExtendedHtml;
    public string Description;
    public string DescriptionHtml;
    public bool ImageWasChanged;
    public LicenseQuestion License;
    public QuestionVisibility Visibility;
    public string Solution;
    public string SolutionDescription;
    public string SolutionMetadataJson;
    public int[]? CommentIds;

    public abstract string ToJson();

    public abstract Question ToQuestion(Question question);

    public abstract QuestionCacheItem ToQuestionCacheItem(int questionId);
}