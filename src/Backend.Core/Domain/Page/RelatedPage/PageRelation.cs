using System.Diagnostics;

[DebuggerDisplay("{Child.Name}({Child.Id}) {Parent.Name}({Parent.Id})")]
[Serializable]
public class PageRelation : DomainEntity
{
    public virtual required Page Child { get; set; } //Child
    public virtual required Page Parent { get; set; } //Parent

    public virtual int? PreviousId { get; set; }
    public virtual int? NextId { get; set; }
}
