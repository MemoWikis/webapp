public class VideoWidgetModel : BaseContentModule
{
    public int SetId;

    public VideoWidgetModel(VideoWidgetJson videoWidgetJson)
    {
        SetId = videoWidgetJson.SetId;
    }

}
