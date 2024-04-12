using NHibernate;
using Seedworks.Lib.Persistence;

public class CategoryRelationRepo(ISession session) : RepositoryDb<CategoryRelation>(session)
{
    public async Task<int> DeleteByRelationIdAsync(int relationId)
    {
        return await _session.CreateSQLQuery(
                "DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + relationId)
            .ExecuteUpdateAsync()
            .ConfigureAwait(false);
    }

    public async Task<int> DeleteByCategoryIdAsync(int categoryId)
    {
        return await _session.CreateSQLQuery(
            "DELETE FROM relatedcategoriestorelatedcategories where Category_id = " +
            categoryId).ExecuteUpdateAsync().ConfigureAwait(false);
    }

    public async Task<int> DeleteQuestionRelationsFromTopic(int categoryId)
    {
        return await _session
            .CreateSQLQuery("DELETE FROM categories_to_questions where Category_id = " + categoryId)
            .ExecuteUpdateAsync()
            .ConfigureAwait(false);
    }
}