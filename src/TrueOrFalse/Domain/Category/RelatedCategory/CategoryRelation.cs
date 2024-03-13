using System.Diagnostics;
using Seedworks.Lib.Persistence;


[DebuggerDisplay("{Child.Name}({Child.Id}) {Parent.Name}({Parent.Id})")]
[Serializable]
public class CategoryRelation : DomainEntity
{
    public virtual required Category Child { get; set; } //Child
    public virtual required Category Parent { get; set; } //Parent

    public virtual int? PreviousId { get; set; }
    public virtual int? NextId { get; set; }
}
