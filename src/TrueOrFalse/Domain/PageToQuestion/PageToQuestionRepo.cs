

using NHibernate;
using System.Text;

public class PageToQuestionRepo(ISession _session)
{
    public void DeleteByPageId(int pageId)
    {
        _session
            .CreateSQLQuery("DELETE FROM pages_to_questions WHERE Page_id = :pageId")
            .SetParameter("pageId", pageId)
            .ExecuteUpdate();

        _session.Flush();
    }

    public void AddQuestionsToPage(int pageId, List<int> questionIds)
    {
        if (questionIds == null || !questionIds.Any())
            throw new ArgumentException("The list of questionIds cannot be null or empty.", nameof(questionIds));

        using (var transaction = _session.BeginTransaction())
        {
            try
            {
                var sql = new StringBuilder("INSERT INTO pages_to_questions (Page_id, Question_id) VALUES ");

                var parameters = questionIds.Select((questionId, index) =>
                    $"(:pageId{index}, :questionId{index})").ToList();

                sql.Append(string.Join(", ", parameters));

                var query = _session.CreateSQLQuery(sql.ToString());

                for (int i = 0; i < questionIds.Count; i++)
                {
                    query.SetParameter($"pageId{i}", pageId);
                    query.SetParameter($"questionId{i}", questionIds[i]);
                }

                query.ExecuteUpdate();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Logg.r.Error("AddQuestionsToPage is failing - {msg}", ex.Message);
                throw new Exception("An error occurred while adding questions to the page.", ex);
            }
        }
    }
}

