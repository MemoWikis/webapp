using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

public class CommentMsg
{
    public static void Send(Comment comment,
        QuestionReadingRepo questionReadingRepo,
        MessageRepo messageRepo,
        IHttpContextAccessor httpContextAccessor,
        IActionContextAccessor actionContextAccessor)
    {
        if (comment.Type != CommentType.AnswerQuestion)
            throw new Exception("Other CommentType than AnswerQuestion is unknown.");

        var question = questionReadingRepo.GetById(comment.TypeId);

        var questionUrl = "";
        if (httpContextAccessor.HttpContext != null)
            questionUrl = new Links(actionContextAccessor, httpContextAccessor).AnswerQuestion(question);

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
                    .Select(x => "<li>" + x + "</li>")
                    .Aggregate((a, b) => a + b));
        }

        if (comment.ShouldRemove)
        {
            shouldImproveOrRemove = string.Format(@"
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

        string body = string.Format(@"
<p>Ein neuer Kommentar auf die Frage <a href='{0}'><i>{1}</i></a>:</p>
{2}
<p>{3}</p>", questionUrl, question.Text, shouldImproveOrRemove, comment.Text.LineBreaksToBRs());

        Send_CommentToYourQuestion(body, receiverUserId: question.Creator.Id, messageRepo);

        if (comment.AnswerTo != null && comment.AnswerTo.Creator.Id != question.Creator.Id)
            Send_AnswerToYourComment(body, comment.AnswerTo.Creator.Id, messageRepo);

        Send_InfoToMemoWikis(body, Constants.MemoWikisAdminUserId, messageRepo);

    }

    public static void Send_CommentToYourQuestion(string body, int receiverUserId, MessageRepo messageRepo)
    {
        messageRepo.Create(new Message
        {
            ReceiverId = receiverUserId,
            Subject = "Ein neuer Kommentar",
            Body = body,
            MessageType = MessageTypes.Comment
        });
    }

    private static void Send_AnswerToYourComment(string body, int receiverUserId, MessageRepo messageRepo)
    {
        messageRepo.Create(new Message
        {
            ReceiverId = receiverUserId,
            Subject = "Antwort auf deinen Kommentar",
            Body = body,
            MessageType = MessageTypes.CommentAnswer
        });
    }

    private static void Send_InfoToMemoWikis(string body, int receiverUserId, MessageRepo messageRepo)
    {
        Send_CommentToYourQuestion(body, receiverUserId, messageRepo);
    }
}