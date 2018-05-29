public class AddToWishknowledge
{
 
    public AddToWishknowledge(bool addToWishknowledge, bool isShortVersion = false)
    {
        IsWishknowledge = addToWishknowledge;
        IsShortVersion = isShortVersion;
    }

    public bool IsWishknowledge;
    public bool IsShortVersion;
}