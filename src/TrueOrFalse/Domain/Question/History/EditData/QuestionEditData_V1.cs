using Newtonsoft.Json;

public class QuestionEditData_V1 : QuestionEditData
{
    //public IList<CategoryRelation_EditData_V2> CategoryRelations;
    //public bool ImageWasUpdated;

    public QuestionEditData_V1(){}

    public QuestionEditData_V1(Question question)
    {
        QuestionText = question.Text;
        QuestionTextExtended = question.TextExtended;
        ImageLicense = question.License;
        Visibility = question.Visibility;
        Solution = question.Solution;
        SolutionDescription = question.Description;
        SolutionType = question.SolutionType;
        SolutionMetadataJson = question.SolutionMetadataJson;
        //ImageWasUpdated = imageWasUpdated;
    }

    public override string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public static QuestionEditData_V1 CreateFromJson(string json)
    {
        return JsonConvert.DeserializeObject<QuestionEditData_V1>(json);
    }

    //public override Category ToCategory(int categoryId)
    //{
    //    var category = Sl.CategoryRepo.GetById(categoryId);

    //    Sl.Session.Evict(category);

    //    category.IsHistoric = true;
    //    category.Name = this.Name;
    //    category.Description = this.Description;
    //    category.TopicMarkdown = this.TopicMardkown;
    //    category.WikipediaURL = this.WikipediaURL;
    //    category.DisableLearningFunctions = this.DisableLearningFunctions;

    //    // Historic category relations cannot be loaded because we do not have archive data and
    //    // loading them leads to nasty conflicts and nuisance with NHibernate.

    //    return category;
    //}
}