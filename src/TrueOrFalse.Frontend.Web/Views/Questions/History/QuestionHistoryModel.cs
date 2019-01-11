using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;

public class QuestionHistoryModel : BaseModel
{
    public int QuestionId { get; set; }
    public string QuestionName;
    public string QuestionUrl;
    public IList<QuestionChangeDayModel> Days;

    public QuestionHistoryModel(Question question, IList<QuestionChange> QuestionChanges)
    {
        QuestionName = question.Text;
        QuestionId = question.Id;
        QuestionUrl = "";//TODO Links.QuestionDetail(QuestionName, QuestionId);

        Days = QuestionChanges
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new QuestionChangeDayModel(
                group.Key,
                (IList<QuestionChange>)group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
    }
}

public class QuestionChangeDayModel
{
    public string Date;
    public IList<QuestionChangeDetailModel> Items;

    public QuestionChangeDayModel(DateTime date, IList<QuestionChange> changes)
    {
        Date = date.ToString("dd.MM.yyyy");
        Items = changes.Select(qc => new QuestionChangeDetailModel
        {
            Author = qc.Author,
            AuthorName = qc.Author.Name,
            AuthorImageUrl = new UserImageSettings(qc.Author.Id).GetUrl_85px_square(qc.Author).Url,
            ElapsedTime = TimeElapsedAsText.Run(qc.DateCreated),
            DateTime = qc.DateCreated.ToString("dd.MM.yyyy HH:mm"),
            Time = qc.DateCreated.ToString("HH:mm"),
            RevisionId = qc.Id,
            QuestionId = qc.Question.Id,
            QuestionName = qc.Question.Text
        }).ToList();
    }
}

public class QuestionChangeDetailModel
{
    public User Author;
    public string AuthorName;
    public string AuthorImageUrl;
    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public int RevisionId;
    public int QuestionId;
    public string QuestionName;
}
