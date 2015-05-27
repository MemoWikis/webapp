using System.Collections.Generic;
using System.Linq;

namespace TrueOrFalse.Tests
{
    public class ContextComment : IRegisterAsInstancePerLifetime
    {
        private readonly CommentRepository _commentRepo;
        private readonly ContextUser _contextUser = ContextUser.New();
        private readonly ContextQuestion _contextQuestion = ContextQuestion.New();

        private readonly User _user1;
        public readonly Question Question;
        
        public List<Comment> All = new List<Comment>();

        public ContextComment(CommentRepository commentRepo){
            _commentRepo = commentRepo;

            _user1 = _contextUser.Add("Test").Persist().All.First();

            Question = _contextQuestion.AddQuestion("text", "solution").Persist().All[0];
        }

        public static ContextComment New()
        {
            return BaseTest.Resolve<ContextComment>();
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
}
