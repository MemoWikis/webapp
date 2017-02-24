public class ForTeachersModel : BaseModel
{
    public int TotalPublicQuestionCount;
    public int TotalSetCount;
    public int TotalCategoryCount;

    public ForTeachersModel()
    {
        TotalPublicQuestionCount = Sl.R<QuestionRepo>().TotalPublicQuestionCount();
        TotalSetCount = Sl.R<SetRepo>().TotalSetCount();
        TotalCategoryCount = Sl.R<CategoryRepository>().TotalCategoryCount();
    }
}