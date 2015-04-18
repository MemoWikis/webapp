using System.Web;

public class GetWishQuestionCountCached : IRegisterAsInstancePerLifetime
{
    private readonly GetWishQuestionCount _getWishQuestionCount;

    public GetWishQuestionCountCached(GetWishQuestionCount getWishQuestionCount){
        _getWishQuestionCount = getWishQuestionCount;
    }

    public int Run(int userId, bool forceReload = false)
    {
        if (!forceReload)
            if (HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount] != null)
                return (int)HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount];

        var result = _getWishQuestionCount.Run(userId);

        HttpContext.Current.Items[ContextItemKeys.WishKnowledgeCount] = result;
        return result;
    }
}