public class PasswordRecoveryToken : DomainEntity
{
    public virtual string Token { get; set; }
    public virtual string Email { get; set; }
}