﻿

using System.Text;
using NHibernate;

public class CategoryToQuestionRepo(ISession _session)
{
    public void DeleteByCategoryId(int categoryId)
    {
        _session
            .CreateSQLQuery("DELETE FROM categories_to_questions WHERE Category_id = :categoryId")
            .SetParameter("categoryId", categoryId)
            .ExecuteUpdate();

        _session.Flush();
    }


    public void AddQuestionsToCategory(int categoryId, List<int> questionIds)
    {
        if (questionIds == null || !questionIds.Any())
            throw new ArgumentException("The list of questionIds cannot be null or empty.", nameof(questionIds));

        using (var transaction = _session.BeginTransaction())
        {
            try
            {
                var sql = new StringBuilder("INSERT INTO categories_to_questions (Category_id, Question_id) VALUES ");

                var parameters = questionIds.Select((questionId, index) =>
                    $"(:categoryId{index}, :questionId{index})").ToList();

                sql.Append(string.Join(", ", parameters));

                var query = _session.CreateSQLQuery(sql.ToString());

                for (int i = 0; i < questionIds.Count; i++)
                {
                    query.SetParameter($"categoryId{i}", categoryId);
                    query.SetParameter($"questionId{i}", questionIds[i]);
                }

                query.ExecuteUpdate();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Logg.r.Error("Questions add to Category is fails - {msg}", ex.Message);
                throw new Exception("An error occurred while adding questions to the category.", ex);
            }
        }
    }

    public void AddQuestionToCategory(int categoryId, int questionId)
    {
        if (questionId == 0)
            throw new ArgumentException("The list of questionIds cannot be have id 0", nameof(questionId));

        using (var transaction = _session.BeginTransaction())
        {
            try
            {
                var sql = new StringBuilder(
                    "INSERT INTO categories_to_questions (Category_id, Question_id) VALUES (:Category_Id, :Question_Id)");
                var query = _session.CreateSQLQuery(sql.ToString());
                query.SetParameter($"Category_Id", categoryId);
                query.SetParameter($"Question_Id", questionId);
                query.ExecuteUpdate();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Logg.r.Error("Questions add to Category is fails - {msg}", ex.Message);
                throw new Exception("An error occurred while adding questions to the category.", ex);
            }
        }
    }
}

