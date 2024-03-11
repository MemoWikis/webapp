using NHibernate;
using NHibernate.Transform;

namespace TrueOrFalse.Updates;

internal class UpdateToVs268
{
    public class CategoryRelationInfo
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ChildId { get; set; }
    }

    public static void Run(ISession nhibernateSession)
    {
        using var transaction = nhibernateSession.BeginTransaction();
        try
        {
            var query = @"SELECT Id, Category_id AS ChildId, Related_id AS ParentId FROM relatedcategoriestorelatedcategories ORDER BY Related_id, Id";
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
                        UpdatePreviousAndNextIds(nhibernateSession, previousRecord.ChildId, currentRecord.ChildId, previousRecord.Id, currentRecord.Id);
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

    private static void UpdatePreviousAndNextIds(ISession session, int previousRecordChildId, int currentRecordChildId, int previousRecordId, int currentRecordId)
    {
        var updateCurrent = @"UPDATE relatedcategoriestorelatedcategories SET Previous_id = :previousChildId WHERE Id = :currentId";
        session.CreateSQLQuery(updateCurrent)
            .SetParameter("previousChildId", previousRecordChildId)
            .SetParameter("currentId", currentRecordId)
            .ExecuteUpdate();

        var updatePrevious = @"UPDATE relatedcategoriestorelatedcategories SET Next_id = :nextChildId WHERE Id = :previousId";
        session.CreateSQLQuery(updatePrevious)
            .SetParameter("nextChildId", currentRecordChildId)
            .SetParameter("previousId", previousRecordId)
            .ExecuteUpdate();
    }
}