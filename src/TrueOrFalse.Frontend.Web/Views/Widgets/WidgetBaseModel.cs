using static System.String;

public class WidgetBaseModel : BaseModel
{
    public string Host;

    public bool IncludeCustomCss => !IsNullOrEmpty(CustomCss);
    public string CustomCss;

    public WidgetBaseModel(string host)
    {
        Host = host;

        if (host == "besser.memucho.de")
            CustomCss = "/Views/Widgets/CustomCss/besser.memucho.de.css";
    }
}