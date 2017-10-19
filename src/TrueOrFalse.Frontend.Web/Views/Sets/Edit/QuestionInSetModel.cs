public class QuestionInSetModel : BaseModel
{
    public int Id { get; }
    public string Text { get; }
    public int CreatorId { get; }
    public string TextExtended { get; }
    public string CorrectAnswer { get; }
    public int TimeCode { get; }
    public int QuestionId { get; }

    public bool IsCreator;

    public QuestionInSetModel(QuestionInSet questionInSet)
    {
        Text =  questionInSet.Question.Text;
        CreatorId = questionInSet.Question.Creator.Id;
        Id = questionInSet.Id;
        TextExtended = questionInSet.Question.TextExtended;
        CorrectAnswer = questionInSet.Question.GetSolution().GetCorrectAnswerAsHtml();
        TimeCode = questionInSet.Timecode;
        QuestionId = questionInSet.Question.Id;

        IsCreator = _sessionUser.IsLoggedInUserOrAdmin(questionInSet.Question.Creator.Id);

    }

}