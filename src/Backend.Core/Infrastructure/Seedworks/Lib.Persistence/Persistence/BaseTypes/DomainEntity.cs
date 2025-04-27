[Serializable]
public class DomainEntity :  Entity, WithDateCreated, WithDateModified
{
    public virtual DateTime DateCreated { get; set; }
    public virtual DateTime DateModified { get; set; }
}