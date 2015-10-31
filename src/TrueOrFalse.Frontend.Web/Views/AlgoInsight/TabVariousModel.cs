using System.Collections.Generic;

public class TabVariousModel : BaseModel
{
    public IList<QuestionFeature> QuestionFeatures;

    public TabVariousModel()
    {
        QuestionFeatures = R<QuestionFeatureRepo>().GetAll();
    }

    public int GetQuestionCount(int featureId)
    {
        return R<QuestionFeatureRepo>().GetFeatureCount(featureId);
    }
}