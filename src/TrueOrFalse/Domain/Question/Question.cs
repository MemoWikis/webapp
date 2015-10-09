using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Seedworks.Lib.Persistence;
using TrueOrFalse;

[DebuggerDisplay("Id={Id} Name={Text}")]
[Serializable]
public class Question : DomainEntity
{
    public virtual string Text { get; set; }
    public virtual string TextExtended { get; set; }
    public virtual string Description { get; set; }
    public virtual string License { get; set; }
    public virtual string Solution { get; set; }
    public virtual SolutionType SolutionType { get; set; }
    public virtual string SolutionMetadataJson { get; set; }

    public virtual IList<Category> Categories { get; set; }
    public virtual IList<Reference> References { get; set; }
    public virtual QuestionVisibility Visibility { get; set; }

    public virtual User Creator { get; set; }

    public virtual int TotalTrueAnswers { get; set; }
    public virtual int TotalFalseAnswers { get; set; }

    public virtual int CorrectnessProbability { get; set; }
    public virtual int CorrectnessProbabilityAnswerCount { get; set; }

    public virtual int TotalAnswers() { return TotalFalseAnswers + TotalTrueAnswers; }
    public virtual int TotalTrueAnswersPercentage()
    {
        if (TotalAnswers() == 0) return 0;
        if (TotalTrueAnswers == 0) return 0;
        return Convert.ToInt32(((decimal)TotalTrueAnswers / TotalAnswers()) * 100);
    }
    public virtual int TotalFalseAnswerPercentage()
    {
        if (TotalAnswers() == 0) return 0;
        if (TotalFalseAnswers == 0) return 0;
        return Convert.ToInt32(((decimal)TotalFalseAnswers / TotalAnswers()) * 100);
    }

    public virtual int TotalQualityAvg { get; set; }
    public virtual int TotalQualityEntries { get; set; }

    public virtual int TotalRelevanceForAllAvg { get; set; }
    public virtual int TotalRelevanceForAllEntries { get; set; }

    public virtual int TotalRelevancePersonalAvg { get; set; }
    public virtual int TotalRelevancePersonalEntries { get; set; }

    public virtual int TotalViews { get; set; }

    public virtual int SetsAmount { get; set; }
    public virtual string SetsTop5Json { get; set; }

    public virtual IList<SetMini> SetTop5Minis
    {
        get
        {
            if(String.IsNullOrEmpty(SetsTop5Json))
                return new List<SetMini>();
            return JsonConvert.DeserializeObject<List<SetMini>>(SetsTop5Json);
        }
        set { SetsTop5Json = JsonConvert.SerializeObject(value);  }
    }

    public virtual bool IsWorkInProgress { get; set; }

    public virtual IList<QuestionFeature> Features { get; set; }

    public Question()
    {
        Categories = new List<Category>();
        References = new List<Reference>();
    }

    public virtual string GetShortTitle(int length = 96) 
    {
        return Text.TruncateAtWord(length);
    }

    public virtual bool IsPrivate()
    {
        return Visibility != QuestionVisibility.All;
    }

    public virtual void UpdateReferences(IList<Reference> references)
    {
        var newReferences = references.Where(r => r.Id == -1 || r.Id == 0).ToArray();
        var removedReferences = References.Where(r => references.All(r2 => r2.Id != r.Id)).ToArray();
        var existingReferenes = references.Where(r => References.Any(r2 => r2.Id == r.Id)).ToArray();

        newReferences.ToList().ForEach(r => { 
            r.DateCreated = DateTime.Now;
            r.DateModified = DateTime.Now;
        });

        for (var i = 0; i < newReferences.Count(); i++)
        {
            newReferences[i].Id = default(Int32);
            References.Add(newReferences[i]);
        }

        for (var i = 0; i < removedReferences.Count(); i++)
            References.Remove(removedReferences[i]);

        for (var i = 0; i < existingReferenes.Count(); i++)
        {
            var reference = References.First(r => r.Id == existingReferenes[i].Id);
            reference.DateModified = DateTime.Now;
            reference.AdditionalInfo = existingReferenes[i].AdditionalInfo;
            reference.ReferenceText = existingReferenes[i].ReferenceText;
        }
    }
}