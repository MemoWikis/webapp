public class IsEmailAddressAvailable : IRegisterAsInstancePerLifetime
{
    private readonly UserRepo _userRepo;

    public IsEmailAddressAvailable(UserRepo userRepo){
        _userRepo = userRepo;
    }

    public bool Yes(string emailAddress){
        return _userRepo.GetByEmail(emailAddress) == null;
    }
}