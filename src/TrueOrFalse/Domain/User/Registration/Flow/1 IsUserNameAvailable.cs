public class IsEmailAddressAvailable : IRegisterAsInstancePerLifetime
{
    private readonly UserRepository _userRepository;

    public IsEmailAddressAvailable(UserRepository userRepository){
        _userRepository = userRepository;
    }

    public bool Yes(string emailAddress){
        return _userRepository.GetByEmail(emailAddress) == null;
    }
}