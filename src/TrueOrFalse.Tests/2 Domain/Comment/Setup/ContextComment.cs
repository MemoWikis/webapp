using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests;

public class ContextComment : IRegisterAsInstancePerLifetime
{
    private readonly CommentRepository _commentRepo;
    private readonly QuestionRepo _questionRepo;
    private readonly UserRepo _userRepo;
    private readonly AnswerQuestion _answerQuestion;
    private readonly ContextUser _contextUser;
    private readonly ContextQuestion _contextQuestion;

    private readonly User _user1;
    public readonly Question Question;
        
    public List<Comment> All = new();

    private ContextComment(CommentRepository commentRepository, 
        QuestionRepo questionRepo,
        UserRepo userRepo,
        AnswerRepo answerRepo,
        AnswerQuestion answerQuestion){
        _commentRepo = commentRepository ;
        _questionRepo = questionRepo;
        _userRepo = userRepo;
        _answerQuestion = answerQuestion;
        _contextUser = ContextUser.New(_userRepo);
        _contextQuestion = ContextQuestion.New(_questionRepo, answerRepo, _answerQuestion);
        _user1 = _contextUser.Add("Test").Persist().All.First();
        Question = _contextQuestion.AddQuestion(questionText: "text", solutionText: "solution").Persist().All[0];
    }


    public ContextComment Add(string text = "My comment", Comment commentTo = null)
    {
        All.Add(new Comment
        {
            TypeId = 1,
            Creator = _user1,
            AnswerTo = commentTo,
            Text = text
        });

        return this;
    }

    public ContextComment Persist()
    {
        foreach (var comment in All)
            _commentRepo.Create(comment);

        return this;
    }
}