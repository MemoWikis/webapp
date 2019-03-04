public class InlineTextModel : BaseContentModule
{
    public string Content;

    public InlineTextModel(InlineTextJson inlineTextJson)
    {
        Content = inlineTextJson.Content;
    }
}
