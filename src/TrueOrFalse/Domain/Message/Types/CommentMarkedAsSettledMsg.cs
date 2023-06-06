using System.Linq;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CommentMarkedAsSettledMsg
{
    public static void Send(int commentId)
    {
        var comment = Sl.R<CommentRepository>().GetById(commentId);
        Send(comment);
    }
    public static void Send(Comment comment)
    {
        if (comment.Type != CommentType.AnswerQuestion)
            throw new Exception("Other CommentType than AnswerQuestion is unknown.");

        var question = Sl.R<QuestionRepo>().GetById(comment.TypeId);

        var questionUrl = Links.AnswerQuestion(question);

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

        string answerToComment = "";
        if (comment.AnswerTo != null)
        {
            var commentAnswerTypeString = "Kommentar";
            if (comment.AnswerTo.ShouldImprove)
                commentAnswerTypeString = "Verbesserungsvorschlag";
            if (comment.AnswerTo.ShouldRemove)
                commentAnswerTypeString = "Löschantrag";
            answerToComment =
                $"<p style='font-size: small;'><i>Dieser Kommentar ist eine Antwort auf den {commentAnswerTypeString} von {comment.AnswerTo.Creator.Name}</i>:<br>{comment.AnswerTo.Text}</p>";
        }

        string body = $@"
<p><b>Frage:</b> <a href='{questionUrl}'><i>{question.Text}</i></a></p>
<p><b>Kommentar:</b></p>
{shouldImproveOrRemove}
<p>{comment.Text.LineBreaksToBRs()}</p>
{answerToComment}";

        var commentAnswerQuestionTypeString = "Kommentar";
        if (comment.ShouldImprove)
            commentAnswerQuestionTypeString = "Verbesserungsvorschlag";
        if (comment.ShouldRemove)
            commentAnswerQuestionTypeString = "Löschantrag";

        var bodyWithAddressing = "";

        bodyWithAddressing =
            $"<p>Dein {commentAnswerQuestionTypeString} auf eine Frage wurde als erledigt markiert.<p>\n{body}";
        Send_YourCommentMarkedAsSettled(bodyWithAddressing, comment.Creator.Id);

        bodyWithAddressing =
            $"<p>Ein {commentAnswerQuestionTypeString} auf deine Frage wurde als erledigt markiert.<p>\n{body}";
        Send_CommentToYourQuestionMarkedAsSettled(bodyWithAddressing, question.Creator.Id);

        foreach (var answer in comment.Answers)
        {
            if (answer.IsSettled || answer.Creator == comment.Creator)
                continue;

            bodyWithAddressing =
                $"<p>Ein {commentAnswerQuestionTypeString}, auf den du geantwortet hast, wurde als erledigt markiert.<p>\n{body}";
            Send_CommentYouHaveAnsweredMarkedAsSettled($"{bodyWithAddressing} <p><b>Deine Antwort:</b><br />{answer.Text}", answer.Creator.Id);
        }
    }

    private static void Send_CommentToYourQuestionMarkedAsSettled(string body, int receiverUserId)
    {
        Sl.R<MessageRepo>().Create(new Message
        {
            ReceiverId = receiverUserId,
            Subject = "Ein Kommentar auf deine Frage wurde als erledigt markiert",
            Body = body,
            MessageType = MessageTypes.CommentMarkedAsSettled
        });
    }

    private static void Send_YourCommentMarkedAsSettled(string body, int receiverUserId)
    {
        Sl.R<MessageRepo>().Create(new Message
        {
            ReceiverId = receiverUserId,
            Subject = "Dein Kommentar wurde als erledigt markiert",
            Body = body,
            MessageType = MessageTypes.CommentMarkedAsSettled
        });
    }

    private static void Send_CommentYouHaveAnsweredMarkedAsSettled(string body, int receiverUserId)
    {
        Sl.R<MessageRepo>().Create(new Message
        {
            ReceiverId = receiverUserId,
            Subject = "Ein Kommentar, auf den du geantwortet hast, wurde als erledigt markiert",
            Body = body,
            MessageType = MessageTypes.CommentMarkedAsSettled
        });
    }

}