using System.Diagnostics;
using Seedworks.Lib.Persistence;


[DebuggerDisplay("{Category.Name}({CategoryId.Id}) {RelatedCategoryId.Name}({RelatedCategory.Id})")]
[Serializable]
public class CategoryRelation : DomainEntity
{
    public virtual Category Category { get; set; } //Child

    public virtual Category RelatedCategory { get; set; } //Parent
}
