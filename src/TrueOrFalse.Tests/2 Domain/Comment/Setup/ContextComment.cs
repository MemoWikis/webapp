using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests;

public class ContextComment
{
    private readonly CommentRepository _commentRepo;
    private readonly User _user1;
        
    public List<Comment> All = new();

    public ContextComment(CommentRepository commentRepository,
        UserReadingRepo userReadingRepo
       ){
        _commentRepo = commentRepository ;
        var userRepo1 = userReadingRepo;
        var contextUser = ContextUser.New(userRepo1);
        _user1 = contextUser.Add("Test").Persist().All.First();
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