public class AddToWishknowledge
{
 
    public AddToWishknowledge(bool addToWishknowledge, bool isShortVersion = false, bool isCircleVersion = false)
    {
        IsWishknowledge = addToWishknowledge;
        IsShortVersion = isShortVersion;
        IsCircleVersion = isCircleVersion;
    }

    public bool IsWishknowledge;
    public bool IsShortVersion;
    public bool IsCircleVersion;
}