using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

[SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
public class TopicLearningQuestionController: BaseController
{
    [HttpPost]
    public JsonResult LoadQuestionBody(int questionId)
    {
        var question = EntityCache.GetQuestionById(questionId);
        var author = new UserTinyModel(question.Creator);
        var authorImage = new UserImageSettings(author.Id).GetUrl_128px_square(author);
        var solution = GetQuestionSolution.Run(question);
        var answerQuestionModel = new AnswerQuestionModel(question, true);
        var history = answerQuestionModel.HistoryAndProbability.AnswerHistory;

        var json = Json(new
        {
            answer = solution.GetCorrectAnswerAsHtml(),
            extendedAnswer = question.DescriptionHtml != null ? question.DescriptionHtml : "",
            categories = question.Categories.Where(c => PermissionCheck.CanViewCategory(c.Id)).Select(c => new
            {
                name = c.Name,
                categoryType = c.Type,
                linkToCategory = Links.CategoryDetail(c),
            }).AsEnumerable().Distinct().ToList(),
            references = question.References.Select(r => new
            {
                referenceType = r.ReferenceType.GetName(),
                additionalInfo = r.AdditionalInfo ?? "",
                referenceText = r.ReferenceText ?? ""
            }).AsEnumerable().Distinct().ToList(),
            author = author.Name,
            authorId = author.Id,
            authorImage = authorImage.Url,
            authorUrl = Links.UserDetail(author),
            extendedQuestion = question.TextExtendedHtml != null ? question.TextExtendedHtml : "",
            commentCount = Resolve<CommentRepository>().GetForDisplay(question.Id)
                .Where(c => !c.IsSettled)
                .Select(c => new CommentModel(c))
                .ToList()
                .Count(),
            isCreator = author.Id == SessionUser.UserId,
            editUrl = Links.EditQuestion(Url, question.Text, question.Id),
            historyUrl = Links.QuestionHistory(question.Id),
            answerCount = history.TimesAnsweredUser,
            correctAnswerCount = history.TimesAnsweredUserTrue,
            wrongAnswerCount = history.TimesAnsweredUserWrong,
            canBeEdited = question.Creator?.Id == SessionUser.UserId || IsInstallationAdmin,
        });

        return json;
    }
}