public class AddToWishknowledge
{

    public bool IsWishknowledge;
    public bool IsShortVersion;
    public bool IsCircleVersion;
    public bool DisplayAdd; 
    public AddToWishknowledge(bool addToWishknowledge, bool isShortVersion = false, bool isCircleVersion = false, bool displayAdd = true)
    {
        IsWishknowledge = addToWishknowledge;
        IsShortVersion = isShortVersion;
        IsCircleVersion = isCircleVersion;
        DisplayAdd = displayAdd;
    }


}