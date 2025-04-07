using Seedworks.Lib.Persistence;

public class Share : DomainEntity
{
    public virtual int PageId { get; set; }
    public virtual int? UserId { get; set; } // null for token-based sharing
    public virtual string Token { get; set; } = "";
    public virtual SharePermission Permission { get; set; } = SharePermission.View;
    public virtual int GrantedBy { get; set; }
}