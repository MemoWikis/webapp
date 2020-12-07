public class AddToWishknowledge
{

    public bool IsWishknowledge;
    public bool IsShortVersion;
    public bool IsCircleVersion;
    public bool DisplayAdd;
    public bool IsHeader;
    public AddToWishknowledge(bool addToWishknowledge, bool isShortVersion = false, bool isCircleVersion = false, bool displayAdd = true, bool isHeader = false)
    {
        IsWishknowledge = addToWishknowledge;
        IsShortVersion = isShortVersion;
        IsCircleVersion = isCircleVersion;
        DisplayAdd = displayAdd;
        IsHeader = isHeader;
    }


}