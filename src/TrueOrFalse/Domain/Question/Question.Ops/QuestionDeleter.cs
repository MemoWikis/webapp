using System.Linq;
using NHibernate;

public class QuestionDeleter : IRegisterAsInstancePerLifetime
{
    private readonly ISession _session;
    private readonly QuestionRepository _questionRepository;
    private readonly AnswerHistoryRepository _answerHistory;
    private readonly UpdateQuestionCountForCategory _updateQuestionCountForCategory;

    public QuestionDeleter(
        QuestionRepository questionRepository, 
        AnswerHistoryRepository answerHistory, 
        UpdateQuestionCountForCategory updateQuestionCountForCategory, 
        ISession session)
    {
        _questionRepository = questionRepository;
        _answerHistory = answerHistory;
        _updateQuestionCountForCategory = updateQuestionCountForCategory;
        _session = session;
    }

    public void Run(int questionId)
    {
        var question = _questionRepository.GetById(questionId);

        ThrowIfNot_IsUserOrAdmin.Run(question.Creator.Id);

        var categoriesToDelete = question.Categories.ToList();
        _questionRepository.Delete(question);
        _updateQuestionCountForCategory.Run(categoriesToDelete);
        _answerHistory.DeleteFor(questionId);

        _session
            .CreateSQLQuery("DELETE FROM categoriestoquestions where Question_id = " + questionId)
            .ExecuteUpdate();
    }

}