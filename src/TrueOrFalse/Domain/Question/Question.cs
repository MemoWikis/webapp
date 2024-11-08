using Seedworks.Lib.Persistence;
using System.Diagnostics;
using System.Text;
using TrueOrFalse;

[DebuggerDisplay("Id={Id} Name={Text}")]
[Serializable]
public class Question : DomainEntity, ICreator
{
    public Question()
    {
        Categories = new List<Page>();
        References = new List<Reference>();
    }

    public virtual IList<Page> Categories { get; set; }
    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }
    public virtual string Description { get; set; }
    public virtual string DescriptionHtml { get; set; }

    public virtual bool IsWorkInProgress { get; set; }

    public virtual LicenseQuestion License
    {
        get => LicenseQuestionRepo.GetById(LicenseId);
        set
        {
            if (value == null)
            {
                return;
            }

            LicenseId = value.Id;
        }
    }

    public virtual int LicenseId { get; set; }
    public virtual IList<Reference> References { get; set; }
    public virtual bool SkipMigration { get; set; }

    public virtual string Solution { get; set; }
    public virtual string SolutionMetadataJson { get; set; }
    public virtual SolutionType SolutionType { get; set; }
    public virtual string Text { get; set; }
    public virtual string TextExtended { get; set; }
    public virtual string TextExtendedHtml { get; set; }

    public virtual string TextHtml { get; set; }
    public virtual int TotalFalseAnswers { get; set; }

    public virtual int TotalQualityAvg { get; set; }
    public virtual int TotalQualityEntries { get; set; }

    public virtual int TotalRelevanceForAllAvg { get; set; }
    public virtual int TotalRelevanceForAllEntries { get; set; }

    public virtual int TotalRelevancePersonalAvg { get; set; }
    public virtual int TotalRelevancePersonalEntries { get; set; }

    public virtual int TotalTrueAnswers { get; set; }

    public virtual QuestionVisibility Visibility { get; set; }

    public virtual User Creator { get; set; }

    public static string AnswersAsHtml(string answerText, SolutionType solutionType)
    {
        switch (solutionType)
        {
            case SolutionType.MatchList:

                //Quick Fix: Prevent null reference exeption
                if (answerText == "" || answerText == null)
                {
                    return "";
                }

                var answerObject = QuestionSolutionMatchList.DeserializeMatchListAnswer(answerText);
                if (answerObject.Pairs.Count == 0)
                {
                    return "(keine Auswahl)";
                }

                var formattedMatchListAnswer = answerObject.Pairs.Aggregate("</br><ul>",
                    (current, pair) => current + "<li>" + pair.ElementLeft.Text + " - " + pair.ElementRight.Text +
                                       "</li>");
                formattedMatchListAnswer += "</ul>";
                return formattedMatchListAnswer;

            case SolutionType.MultipleChoice:
                if (answerText == "")
                {
                    return "(keine Auswahl)";
                }

                var builder = new StringBuilder(answerText);
                var formattedMultipleChoiceAnswer = "</br> <ul> <li>" +
                                                    builder.Replace("%seperate&xyz%", "</li><li>") +
                                                    "</li> </ul>";
                return formattedMultipleChoiceAnswer;
        }

        return answerText;
    }

    public virtual int TotalAnswers()
    {
        return TotalFalseAnswers + TotalTrueAnswers;
    }

    public virtual bool IsCreator(int userId)
    {
        return userId == Creator?.Id;
    }
}