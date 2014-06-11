using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Infrastructure;

namespace TrueOrFalse
{
    public class SendCommentMsg : BaseSendMessage, IRegisterAsInstancePerLifetime
    {
        private readonly QuestionRepository _questionRepo;

        public SendCommentMsg(
            MessageRepository messageRepo, 
            UserRepository userRepo, 
            QuestionRepository questionRepo)
            : base(messageRepo, userRepo)
        {
            _questionRepo = questionRepo;
        }

        public void Run(Comment comment)
        {
            var question = _questionRepo.GetById(comment.TypeId);

            var questionUrl = "";
            if(HttpContext.Current != null)
                questionUrl = Links.AnswerQuestion(new UrlHelper(HttpContext.Current.Request.RequestContext), question);

            string body = String.Format(@"
<p>Ein neuer Kommentar auf die Frage <a href='{0}'><i>'{1}'</i></a>: .</p>

<p>{2}</p>", questionUrl, question.Text, comment.Text.LineBreaksToBRs());

            _messageRepo.Create(new Message
            {
                ReceiverId = comment.Creator.Id,
                Subject = "Ein neuer Kommentar",
                Body = body,
                MessageType = "NewComment"
            });
        }
    }
}
