using Microsoft.AspNetCore.Mvc;

public class VueLearningSessionResultController(LearningSessionResultService _learningSessionResultService) : Controller
{
    [HttpGet]
    public LearningSessionResultService.LearningSessionResult Get()
    {
      return _learningSessionResultService.GetLearningSessionResult();
    }
}