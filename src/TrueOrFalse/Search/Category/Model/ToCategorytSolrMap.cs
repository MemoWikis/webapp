namespace TrueOrFalse.Search
{
    public class ToCategorytSolrMap
    {
        public static CategorySolrMap Run(Category category)
        {
            var result = new CategorySolrMap();
            result.Id = category.Id;
            result.Name = category.Name;
            result.Description = category.Description;
            result.CreatorId = category.Creator.Id;
            result.QuestionCount = category.CountQuestions;
            result.DateCreated = category.DateCreated;

            return result;
        }
    }
}
