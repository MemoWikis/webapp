using System;
using System.Collections.Generic;
using Seedworks.Lib.Persistence;

[Serializable]
public class QuestionFeature : DomainEntity
{
    public virtual string Id2 { get; set; }

    public virtual string Name { get; set; }
    public virtual string Description { get; set; }

    public virtual IList<Question> Questions { get; set; }

    public virtual string Group { get; set; }

    public virtual Func<QuestionFeatureFilterParams, bool> DoesApply { get; set; }
}