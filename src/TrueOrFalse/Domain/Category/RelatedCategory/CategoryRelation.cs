using System.Diagnostics;
using Seedworks.Lib.Persistence;


[DebuggerDisplay("{Child.Name}({CategoryId.Id}) {RelatedCategoryId.Name}({Parent.Id})")]
[Serializable]
public class CategoryRelation : DomainEntity
{
    public virtual required Category Child { get; set; } //Child

    public virtual required Category Parent { get; set; } //Parent
}
