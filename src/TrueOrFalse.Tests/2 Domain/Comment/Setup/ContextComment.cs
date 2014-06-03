using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse.Tests
{
    public class ContextComment : IRegisterAsInstancePerLifetime
    {
        private readonly CommentRepository _commentRepo;
        private readonly ContextUser _contextUser = ContextUser.New();

        private readonly User _user1;
        
        public List<Comment> All = new List<Comment>();

        public ContextComment(CommentRepository commentRepo){
            _commentRepo = commentRepo;

            _user1 = _contextUser.Add("Test").Persist().All.First();
        }

        public static ContextComment New()
        {
            return BaseTest.Resolve<ContextComment>();
        }

        public ContextComment Add(string text = "My comment", Comment comment = null)
        {
            All.Add(new Comment
            {
                TypeId = 1,
                User = _user1,
                AnswerTo = comment,
                Text = "My comment"
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
