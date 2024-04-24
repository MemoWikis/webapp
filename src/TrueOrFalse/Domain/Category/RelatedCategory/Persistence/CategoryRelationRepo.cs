using MySql.Data.MySqlClient;
using NHibernate;
using Seedworks.Lib.Persistence;

public class CategoryRelationRepo : RepositoryDb<CategoryRelation>
{
    public CategoryRelationRepo(ISession session) : base(session)
    {
    }

    public List<CategoryRelation> GetByRelationId(int relationId)
    {
        return Session.QueryOver<CategoryRelation>()
            .Where(r => r.Parent.Id == relationId)
            .List()
            .ToList();
    }

    public async Task DeleteByRelationId(int relationId)
    {
        await _session.CreateSQLQuery(
                "DELETE FROM relatedcategoriestorelatedcategories where Id = " + relationId)
            .ExecuteUpdateAsync()
            .ConfigureAwait(false);
    }

    public async Task<int> DeleteByParentIdAsync(int parentId)
    {
        return await _session.CreateSQLQuery(
                "DELETE FROM relatedcategoriestorelatedcategories where Related_id = " + parentId)
            .ExecuteUpdateAsync()
            .ConfigureAwait(false);
    }

    public async Task<int> DeleteByChildIdAsync(int childId)
    {
        return await _session.CreateSQLQuery(
            "DELETE FROM relatedcategoriestorelatedcategories where Category_id = " +
            childId).ExecuteUpdateAsync().ConfigureAwait(false);
    }

    public async Task UpdateRelationAsync(
        int relationId,
        int childId,
        int parentId,
        int? previousId = null,
        int? nextId = null)
    {
        await _session
            .CreateSQLQuery("UPDATE relatedcategoriestorelatedcategories SET " +
                            "ChildId = " + childId +
                            ", ParentId = " + parentId +
                            ", PreviousId = " + previousId +
                            ", NextId = " + nextId +
                            " WHERE Id = " + relationId)
            .ExecuteUpdateAsync()
            .ConfigureAwait(false);
    }
}