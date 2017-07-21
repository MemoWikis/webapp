using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Seedworks.Lib;
using TrueOrFalse.Web;

public class QuestionInSetModel : BaseModel
{

    public int Id { get; private set; }
    public string Text { get; private set; }
    public int CreatorId { get; private set; }
    public string TextExtended { get; private set; }
    public string CorrectAnswer { get; private set; }
    public int TimeCode { get; private set; }
    public int QuestionInSetId { get; private set; }

    public QuestionInSetModel(QuestionInSet questionInSet)
    {
        Text =  questionInSet.Question.Text;
        CreatorId = questionInSet.Question.Creator.Id;
        Id = questionInSet.Id;
        TextExtended = questionInSet.Question.TextExtended;
        CorrectAnswer = questionInSet.Question.GetSolution().GetCorrectAnswerAsHtml();
        TimeCode = questionInSet.Timecode;
        QuestionInSetId = questionInSet.Id;

    }

}