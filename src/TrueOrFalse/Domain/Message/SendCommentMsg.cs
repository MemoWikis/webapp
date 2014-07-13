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

            string shouldImproveOrRemove = "";
            if (comment.ShouldImprove)
            {
                shouldImproveOrRemove = String.Format(@"
                    <p>Die Frage sollte verbessert werden!</p>
                    <div class='ReasonList'>
                        <i class='fa fa-repeat show-tooltip' style='float:left' title='Die Frage sollte verbessert werden'></i>&nbsp;
                        <ul style='float: left; position: relative; top: -3px; padding-left: 10px; list-style-type: none;'>
                            {0}
                        </ul>
                    </div>",
                    ShouldReasons
                        .ByKeys(comment.ShouldKeys)
                        .Select(x => "<li>" + x +"</li>")
                        .Aggregate((a, b) => a + b));
            }

            if (comment.ShouldRemove)
            {
                shouldImproveOrRemove = String.Format(@"
                    <p>Die Frage sollte entfernt werden!</p>
                    <div class='ReasonList'>
                        <i class='fa fa-fire show-tooltip' style='float:left' title='Die Frage sollte entfernt werden'></i>&nbsp;
                        <ul style='float: left; position: relative; top: -3px; padding-left: 10px; list-style-type: none;'>
                            {0}
                        </ul>
                    </div>",
                    ShouldReasons
                        .ByKeys(comment.ShouldKeys)
                        .Select(x => "<li>" + x + "</li>")
                        .Aggregate((a, b) => a + b));                
            }

            string body = String.Format(@"
<p>Ein neuer Kommentar auf die Frage <a href='{0}'><i>'{1}'</i></a>: .</p>
{2}
<p>{3}</p>", questionUrl, question.Text, shouldImproveOrRemove, comment.Text.LineBreaksToBRs());

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
