public class PersistentLogin
{
    public virtual int Id { get; set; }
    public virtual int UserId { get; set; }
        
    /// <summary>Hashed</summary>
    public virtual string LoginGuid { get; set; }

    public virtual DateTime Created { get; set; }
}