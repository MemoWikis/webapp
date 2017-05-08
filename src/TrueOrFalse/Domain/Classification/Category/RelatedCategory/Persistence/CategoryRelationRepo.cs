using System.Collections.Generic;
using NHibernate;
using Seedworks.Lib.Persistence;
using Seedworks.Lib.ValueObjects;

public class CategoryRelationRepo : RepositoryDb<CategoryRelation>
{
    public CategoryRelationRepo(ISession session) : base(session) { }

    //public override void Delete(int categoryRelationId)
    //{
    //    var questionInSet = GetById(categoryRelationId);
    //    base.Delete(categoryRelationId);
    //}

    public void DeleteForCategory(int categoryId)
    {
        //Session.CreateSQLQuery("DELETE FROM questioninset WHERE Question_id = :questionId")
        //        .SetParameter("questionId", categoryId).ExecuteUpdate();
    }

    //public IList<CategoryRelation> GetForCategory(int categoryId)
    //{
    //    return
    //}
}