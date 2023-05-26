﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Seedworks.Lib.Persistence;
using TrueOrFalse;

[DebuggerDisplay("Id={Id} Name={Text}")]
[Serializable]
public class Question : DomainEntity, ICreator
{
    public Question()
    {
        Categories = new List<Category>();
        References = new List<Reference>();
    }

    public virtual IList<Category> Categories { get; set; }

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

    public virtual int TotalViews { get; set; }
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

    public virtual IEnumerable<Category> CategoriesVisibleToCurrentUser()
    {
        return Categories.Where(PermissionCheck.CanView);
    }

    public virtual string GetShortTitle(int length = 96)
    {
        var safeText = Regex.Replace(Text, "<.*?>", "");
        return safeText.TruncateAtWord(length);
    }

    public virtual QuestionSolution GetSolution()
    {
        return GetQuestionSolution.Run(Id);
    }

    public virtual bool IsEasyQuestion()
    {
        return false;
    }

    public virtual bool IsHardQuestion()
    {
        return false;
    }

    public virtual bool IsInWishknowledge()
    {
        return SessionUserCache.IsQuestionInWishknowledge(Sl.CurrentUserId, Id);
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

    public virtual string ToLomXml()
    {
        return LomXml.From(this);
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

    public virtual void UpdateReferences(IList<ReferenceCacheItem> references)
    {
        var newReferences = references.Where(r => r.Id == -1 || r.Id == 0).ToArray();
        var removedReferences = References.Where(r => references.All(r2 => r2.Id != r.Id)).ToArray();
        var existingReferences = references.Where(r => References.Any(r2 => r2.Id == r.Id)).ToArray();

        newReferences.ToList().ForEach(r =>
        {
            r.DateCreated = DateTime.Now;
            r.DateModified = DateTime.Now;
        });

        for (var i = 0; i < newReferences.Count(); i++)
        {
            newReferences[i].Id = default;
            var currentReference = newReferences[i];
            References.Add(new Reference
            {
                AdditionalInfo = currentReference.AdditionalInfo,
                Category = Sl.CategoryRepo.GetByIdEager(currentReference.Category),
                Id = currentReference.Id,
                DateCreated = currentReference.DateCreated,
                DateModified = currentReference.DateModified,
                Question = Sl.QuestionRepo.GetById(currentReference.Question.Id),
                ReferenceText = currentReference.ReferenceText,
                ReferenceType = currentReference.ReferenceType
            });
        }

        for (var i = 0; i < removedReferences.Count(); i++)
        {
            References.Remove(removedReferences[i]);
        }

        for (var i = 0; i < existingReferences.Count(); i++)
        {
            var reference = References.First(r => r.Id == existingReferences[i].Id);
            reference.DateModified = DateTime.Now;
            reference.AdditionalInfo = existingReferences[i].AdditionalInfo;
            reference.ReferenceText = existingReferences[i].ReferenceText;
        }
    }
}