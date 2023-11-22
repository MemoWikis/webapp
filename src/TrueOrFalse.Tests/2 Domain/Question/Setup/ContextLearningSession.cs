using System.Collections.Generic;
using System.Linq;
using TrueOrFalse.Tests;

internal class ContextLearningSession 
{
    private readonly CategoryRepository _categoryRepository;
    private readonly LearningSessionCreator _learningSessionCreator;
    private readonly AnswerRepo _answerRepo;
    private readonly AnswerQuestion _answerQuestion;
    private readonly LearningSessionConfig _learningSessionConfig;
    private readonly QuestionWritingRepo _questionWritingRepo;
    private readonly UserWritingRepo _userWritingRepo;

    public ContextLearningSession(
        CategoryRepository categoryRepository,
        LearningSessionCreator learningSessionCreator,
        AnswerRepo answerRepo,
        AnswerQuestion answerQuestion,
        LearningSessionConfig learningSessionConfig,
        QuestionWritingRepo questionWritingRepo,
         UserWritingRepo userWritingRepo)
    {
        _categoryRepository = categoryRepository;
        _learningSessionCreator = learningSessionCreator;
        _answerRepo = answerRepo;
        _answerQuestion = answerQuestion;
        _learningSessionConfig = learningSessionConfig;
        _questionWritingRepo = questionWritingRepo;
        _userWritingRepo = userWritingRepo;
    }
    public List<LearningSessionStep> GetSteps(int amountQuestionInMemory,
        int amountQuestions = 20)
    {
        var learningSession = GetLearningSessionForAnonymusUser(amountQuestionInMemory,amountQuestions);

        return learningSession.Steps.ToList();
    }

    public LearningSession GetLearningSessionForAnonymusUser(int amountQuestions, int amountQuestionInMemory = 20)
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(_categoryRepository, _answerRepo, _answerQuestion, _userWritingRepo, _questionWritingRepo, amountQuestionInMemory);
        var learningSession =_learningSessionCreator.BuildLearningSession(
            new LearningSessionConfig
            {
                CategoryId = 1,
                MaxQuestionCount = amountQuestions,
            });
        return learningSession;
    }

    public LearningSession GetLearningSessionWithUser()
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(_categoryRepository,
            _answerRepo, 
            _answerQuestion,
            _userWritingRepo, 
            _questionWritingRepo, 
            _learningSessionConfig.MaxQuestionCount);
        return new LearningSession(GetSteps(_learningSessionConfig.MaxQuestionCount), _learningSessionConfig);
    }

    public LearningSession GetLearningSession()
    {
        ContextQuestion.PutQuestionsIntoMemoryCache(_categoryRepository,
            _answerRepo, 
            _answerQuestion,
            _userWritingRepo, 
            _questionWritingRepo);
        return _learningSessionCreator.BuildLearningSession(_learningSessionConfig);
    }
}