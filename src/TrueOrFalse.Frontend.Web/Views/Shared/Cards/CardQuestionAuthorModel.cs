using System;
using System.Collections.Generic;


public class CardQuestionAuthorModel : UserCardBaseModel 
{
    public Question Question;
    public AuthorViewModel AuthorViewModel;
    public IList<AuthorViewModel> Authors;
    public User User;
    public QuestionRepo QuestionRepo;


    public CardQuestionAuthorModel(Question question)
    {
        Authors = new List<AuthorViewModel>();
        QuestionRepo = Sl.QuestionRepo;

        var _questionRepo = R<QuestionRepo>();
        var authors = _questionRepo.GetAuthorsQuestion(question.Id);

        Authors = AuthorViewModel.Convert(authors);

        FillUserCardBaseModel(authors, User.Id);
    }
}