using Markdig.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VueApp;

public class TopicController : BaseController
{
    private readonly TopicControllerLogic _topicControllerLogic;
    private readonly CategoryViewRepo _categoryViewRepo;

    public TopicController(SessionUser sessionUser,
        TopicControllerLogic topicControllerLogic, CategoryViewRepo categoryViewRepo) : base(sessionUser)
    {
        _topicControllerLogic = topicControllerLogic;
        _categoryViewRepo = categoryViewRepo;
    }

    [HttpGet]
    public JsonResult GetTopic([FromRoute] int id)
    {
        var userAgent = Request.Headers["User-Agent"].ToString();
        _categoryViewRepo.AddView(userAgent, id, _sessionUser.UserId);
  
        GetAllChildrenTime(id);
        GetChildrenTime(id);

        return Json(_topicControllerLogic.GetTopicData(id));
    }

    public void GetChildrenTime(int id)
    {
        var timer = new Stopwatch();
        timer.Start();
        EntityCache.GetChildren(id);
        timer.Stop();
        Logg.r.Information("New GetChildren - time: {time}, topicId: {id}", timer.Elapsed, id);

        var timerOld = new Stopwatch();
        timerOld.Start();
        EntityCache.GetChildrenOld(id);
        timerOld.Stop();
        Logg.r.Information("Old GetChildren - time: {time}, topicId: {id}", timerOld.Elapsed, id);

        var percentageDecrease = ((timerOld.ElapsedTicks - timer.ElapsedTicks) / (double)timerOld.ElapsedTicks) * 100;

        Logg.r.Information("Time difference GetChildren: {difference}, topicId: {id}, percentage decrease: {percentage}%", timerOld.Elapsed - timer.Elapsed, id, percentageDecrease);
    }
    public void GetAllChildrenTime(int id)
    {
        Stopwatch timer = Stopwatch.StartNew();
        timer.Start();
        EntityCache.GetAllChildren(id);
        timer.Stop();
        Logg.r.Information("New GetAllChildren - time: {time}, topicId: {id}", timer.Elapsed, id);

        var timerOld = new Stopwatch();
        timerOld.Start();
        EntityCache.GetAllChildrenOld(id);
        timerOld.Stop();
        Logg.r.Information("Old GetAllChildren - time: {time}, topicId: {id}", timerOld.Elapsed, id);

        var percentageDecrease = (timerOld.ElapsedTicks - timer.ElapsedTicks) / timerOld.ElapsedTicks * 100;

        Logg.r.Information("Time difference GetAllChildren: {difference}, topicId: {id}, percentage decrease: {percentage}%", timerOld.Elapsed - timer.Elapsed, id, percentageDecrease);
    }
}

