using NHibernate;
using NHibernate.Transform;

namespace TrueOrFalse.Updates;

internal class UpdateToVs268
{
    public class CategoryRelationInfo
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
    }

    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();
        try
        {
            var query = @"SELECT Id, Related_id AS ParentId FROM relatedcategoriestorelatedcategories ORDER BY Related_id, Id";
            IList<CategoryRelationInfo> allRecords = nhibernateSession.CreateSQLQuery(query)
                .SetResultTransformer(Transformers.AliasToBean<CategoryRelationInfo>())
                .List<CategoryRelationInfo>();

            var groupedRecords = allRecords.GroupBy(x => x.ParentId);

            foreach (var group in groupedRecords)
            {
                CategoryRelationInfo previousRecord = null;
                foreach (var currentRecord in group)
                {
                    if (previousRecord != null)
                    {
                        UpdatePreviousAndNextIds(nhibernateSession, previousRecord.Id, currentRecord.Id);
                    }
                    previousRecord = currentRecord;
                }
            }

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }

    private static void UpdatePreviousAndNextIds(ISession session, int previousRecordId, int currentRecordId)
    {
        var updateCurrent = @"UPDATE relatedcategoriestorelatedcategories SET Previous_id = :previousId WHERE Id = :currentId";
        session.CreateSQLQuery(updateCurrent)
            .SetParameter("previousId", previousRecordId)
            .SetParameter("currentId", currentRecordId)
            .ExecuteUpdate();

        var updatePrevious = @"UPDATE relatedcategoriestorelatedcategories SET Next_id = :nextId WHERE Id = :previousId";
        session.CreateSQLQuery(updatePrevious)
            .SetParameter("nextId", currentRecordId)
            .SetParameter("previousId", previousRecordId)
            .ExecuteUpdate();
    }
}