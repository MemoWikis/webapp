public class WidgetMenuModel : BaseModel
{
    public bool CurrentIsHelp => !CurrentIsPricing && !CurrentIsExample;
    public bool CurrentIsPricing;
    public bool CurrentIsExample;
}