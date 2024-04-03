using Microsoft.AspNetCore.Mvc;
using TrueOrFalse.Domain.Question.Answer;

public class AnswerQuestionDetailsController(AnswerQuestionDetailsService _answerQuestionDetailsService): Controller
{
    [HttpGet]
    public AnswerQuestionDetailsResult? Get([FromRoute] int id) => _answerQuestionDetailsService.GetData(id);
}