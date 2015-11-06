using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("{Group} {Name}")]
public class AnswerFeature : DomainEntity
{
    public virtual string Id2 { get; set; }

    public virtual string Group { get; set; }
    public virtual string Name { get; set; }
    public virtual string Description { get; set; }

    public virtual IList<Answer> Answers { get; set; }

    public virtual Func<AnswerFeatureFilterParams, bool> DoesApply { get; set; }
}

public static class AnswerFeaturesExt
{
    public static AnswerFeature ByName(this IEnumerable<AnswerFeature> answerFeatures, string name)
    {
        return answerFeatures.First(feature => feature.Name == name);
    }
}