using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TrueOrFalse;

[DebuggerDisplay("Id={Id} Name={Name}")]
[Serializable]
public class QuestionCacheItem
{
    public QuestionCacheItem()
    {
        Categories = new List<CategoryCacheItem>();
        References = new List<ReferenceCacheItem>();
    }

    public virtual UserCacheItem Creator => EntityCache.GetUserById(CreatorId);

    public virtual IList<CategoryCacheItem> Categories { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public int CreatorId { get; set; }
    public virtual DateTime DateCreated { get; set; }
    public virtual DateTime DateModified { get; set; }
    public virtual string Description { get; set; }
    public virtual string DescriptionHtml { get; set; }
    public virtual int Id { get; set; }

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
    public virtual IList<ReferenceCacheItem> References { get; set; }
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

    public virtual int TotalViews { get; set; }
    public virtual QuestionVisibility Visibility { get; set; }

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

   

    public virtual string GetShortTitle(int length = 96)
    {
        var safeText = Regex.Replace(Text, "<.*?>", "");
        return safeText.TruncateAtWord(length);
    }

    public virtual QuestionSolution GetSolution()
    {
        return GetQuestionSolution.Run(this);
    }

    public virtual bool IsEasyQuestion()
    {
        return false;
    }

    public virtual bool IsHardQuestion()
    {
        return false;
    }
    public virtual IEnumerable<CategoryCacheItem> CategoriesVisibleToCurrentUser(PermissionCheck permissionCheck)
    {
        return Categories.Where(permissionCheck.CanView);
    }
    public virtual bool IsInWishknowledge(int userId, CategoryValuationRepo categoryValuationRepo, UserRepo userRepo, QuestionValuationRepo questionValuationRepo)
    {
        return SessionUserCache.IsQuestionInWishknowledge(userId, Id, categoryValuationRepo, userRepo, questionValuationRepo);
    }

    public virtual bool IsMediumQuestion()
    {
        return false;
    }

    public virtual bool IsNobrainer()
    {
        return false;
    }

    public virtual bool IsPrivate()
    {
        return Visibility != QuestionVisibility.All;
    }

    public static IEnumerable<QuestionCacheItem> ToCacheCategories(IEnumerable<Question> questions)
    {
        return questions.Select(q => ToCacheQuestion(q));
    }

    public static QuestionCacheItem ToCacheQuestion(Question question)
    {
        var questionCacheItem = new QuestionCacheItem
        {
            Id = question.Id,
            CorrectnessProbability = question.CorrectnessProbability,
            CorrectnessProbabilityAnswerCount = question.CorrectnessProbabilityAnswerCount,
            Description = question.Description,
            SkipMigration = question.SkipMigration,
            Visibility = question.Visibility,
            TotalRelevancePersonalEntries = question.TotalRelevancePersonalEntries,
            Categories = EntityCache.GetCategories(question.Categories?.Select(c => c.Id)).ToList(),
            CreatorId = question.Creator?.Id ?? -1,
            DateCreated = question.DateCreated,
            DateModified = question.DateModified,
            DescriptionHtml = question.DescriptionHtml,
            TotalQualityAvg = question.TotalQualityAvg,
            TotalQualityEntries = question.TotalQualityEntries,
            IsWorkInProgress = question.IsWorkInProgress,
            TextExtended = question.TextExtended,
            TextExtendedHtml = question.TextExtendedHtml,
            TotalRelevanceForAllEntries = question.TotalRelevanceForAllEntries,
            TotalRelevanceForAllAvg = question.TotalRelevanceForAllAvg,
            TotalRelevancePersonalAvg = question.TotalRelevancePersonalAvg,
            Text = question.Text,
            TextHtml = question.TextHtml,
            TotalFalseAnswers = question.TotalFalseAnswers,
            TotalTrueAnswers = question.TotalTrueAnswers,
            TotalViews = question.TotalViews,
            SolutionType = question.SolutionType,
            LicenseId = question.LicenseId,
            Solution = question.Solution,
            SolutionMetadataJson = question.SolutionMetadataJson,
            License = question.License
        };
        if (!EntityCache.IsFirstStart)
        {
            questionCacheItem.References = ReferenceCacheItem.ToReferenceCacheItems(question.References).ToList();
        }

        return questionCacheItem;
    }

    public static IEnumerable<QuestionCacheItem> ToCacheQuestions(List<Question> questions)
    {
        return questions.Select(q => ToCacheQuestion(q));
    }

    public static IEnumerable<QuestionCacheItem> ToCacheQuestions(IList<Question> questions)
    {
        return questions.Select(q => ToCacheQuestion(q));
    }

    public virtual string ToLomXml(CategoryRepository categoryRepository)
    {
        return LomXml.From(this, categoryRepository);
    }

    public virtual int TotalAnswers()
    {
        return TotalFalseAnswers + TotalTrueAnswers;
    }

    public virtual int TotalFalseAnswerPercentage()
    {
        if (TotalAnswers() == 0)
        {
            return 0;
        }

        if (TotalFalseAnswers == 0)
        {
            return 0;
        }

        return Convert.ToInt32((decimal)TotalFalseAnswers / TotalAnswers() * 100);
    }

    public virtual int TotalTrueAnswersPercentage()
    {
        if (TotalAnswers() == 0)
        {
            return 0;
        }

        if (TotalTrueAnswers == 0)
        {
            return 0;
        }

        return Convert.ToInt32((decimal)TotalTrueAnswers / TotalAnswers() * 100);
    }

    public virtual void UpdateReferences(IList<Reference> references)
    {
        var newReferences = ReferenceCacheItem
            .ToReferenceCacheItems(references.Where(r => r.Id == -1 || r.Id == 0).ToList()).ToArray();
        var removedReferences = References.Where(r => references.All(r2 => r2.Id != r.Id)).ToArray();
        var existingReferenes = references.Where(r => References.Any(r2 => r2.Id == r.Id)).ToArray();

        newReferences.ToList().ForEach(r =>
        {
            r.DateCreated = DateTime.Now;
            r.DateModified = DateTime.Now;
        });

        for (var i = 0; i < newReferences.Count(); i++)
        {
            newReferences[i].Id = default;
            References.Add(newReferences[i]);
        }

        for (var i = 0; i < removedReferences.Count(); i++)
        {
            References.Remove(removedReferences[i]);
        }

        for (var i = 0; i < existingReferenes.Count(); i++)
        {
            var reference = References.First(r => r.Id == existingReferenes[i].Id);
            reference.DateModified = DateTime.Now;
            reference.AdditionalInfo = existingReferenes[i].AdditionalInfo;
            reference.ReferenceText = existingReferenes[i].ReferenceText;
        }
    }
    public virtual bool IsCreator(int userId)
    {
        return userId == Creator?.Id;
    }
}