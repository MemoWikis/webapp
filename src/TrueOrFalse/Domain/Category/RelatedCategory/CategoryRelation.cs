using System;
using System.Diagnostics;
using Seedworks.Lib.Persistence;

[DebuggerDisplay("{Category.Name}({CategoryId.Id}) [{CategoryRelationType.ToString()}] {RelatedCategoryId.Name}({RelatedCategory.Id})")]
[Serializable]
public class CategoryRelation : DomainEntity
{
    public virtual int CategoryId { get; set; }

    public virtual int RelatedCategoryId { get; set; }

    public virtual CategoryRelationType CategoryRelationType { get; set; }
}
