using System.Web.Mvc;

[Serializable]
public class QuestionHistory : HistoryBase<QuestionHistoryItem>
{
    public QuestionHistory()
    {
        _size = 5;
    }
}

[Serializable]
public class QuestionHistoryItem : HistoryItemBase
{
    public int Id { get; private set; }
    public int SetId { get; private set; }
    public string Text { get; private set; }
    public string Solution { get; private set; }
    public HistoryItemType Type { get; set; }
        
    public QuestionSearchSpec SearchSpec { get; set; }

    public Func<UrlHelper, string> Link;

    public Question Question;
    public Set Set;

    public QuestionHistoryItem(
        Set set,
        QuestionCacheItem question,
        HistoryItemType type = HistoryItemType.Any)
    {
        Set = set;
        SetId = set.Id;
        Type = type;
            
        FillQuestionFields(question);
    }

    public QuestionHistoryItem(
        QuestionCacheItem question, 
        QuestionSearchSpec seachSpec,
        HistoryItemType type = HistoryItemType.Any)
    {
        Type = type;
        FillQuestionFields(question);
            
        SearchSpec = QuestionSearchSpecSession.AddCloneToSession(seachSpec, this);
    }

    public QuestionHistoryItem(QuestionCacheItem question, HistoryItemType type)
    {
        Type = type;
            
        FillQuestionFields(question);
    }

    private void FillQuestionFields(QuestionCacheItem question)
    {
        Question = Sl.QuestionRepo.GetById(question.Id);
        Id = question.Id;
        Text = question.Text;
        Solution = GetQuestionSolution.Run(question.Id).GetCorrectAnswerAsHtml();
    }
}