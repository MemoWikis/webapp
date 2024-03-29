﻿using System;
using Microsoft.AspNetCore.Mvc;

public class ActivityPointsStoreController : BaseController
{
    private readonly ActivityPointsRepo _activityPointsRepo;
    private readonly UserWritingRepo _userWritingRepo;

    public ActivityPointsStoreController(SessionUser sessionUser,
        ActivityPointsRepo activityPointsRepo,
        UserWritingRepo userWritingRepo) : base(sessionUser)
    {
        _activityPointsRepo = activityPointsRepo;
        _userWritingRepo = userWritingRepo;
    }

    public readonly record struct AddJson(string ActivityTypeString, int Points);
    [HttpPost]
    public JsonResult Add([FromBody] AddJson activityPointsData)
    {
        var activityType = (ActivityPointsType)Enum.Parse(typeof(ActivityPointsType), activityPointsData.ActivityTypeString);
        var activityPoints = new ActivityPoints
        {
            Amount = activityPointsData.Points,
            ActionType = activityType,
            DateEarned = DateTime.Now
        };

        if (_sessionUser.IsLoggedIn)
        {
            var oldUserLevel = _sessionUser.User.ActivityLevel;
            activityPoints.UserId = _sessionUser.UserId;
            _activityPointsRepo.Create(activityPoints);
            _userWritingRepo.UpdateActivityPointsData();

            var activityLevel = _sessionUser.User.ActivityLevel;
            var activityPointsAtNextLevel = UserLevelCalculator.GetUpperLevelBound(activityLevel);
            var activityPointsTillNextLevel = activityPointsAtNextLevel - _sessionUser.User.ActivityPoints;
            var activityPointsPercentageOfNextLevel = _sessionUser.User.ActivityPoints == 0 ? 0 : 100 * _sessionUser.User.ActivityPoints / activityPointsAtNextLevel;

            return Json(new 
                {
                    points = _sessionUser.User.ActivityPoints,
                    level = activityLevel,
                    levelUp = oldUserLevel < activityLevel,
                    activityPointsTillNextLevel = activityPointsTillNextLevel,
                    activityPointsPercentageOfNextLevel
                });
        } 
        else 
        {
            var oldUserLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());
            _sessionUser.AddPointActivity(activityPoints);
            var newUserLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());

            return Json(new
                {
                    points = _sessionUser.GetTotalActivityPoints(),
                    level = newUserLevel,
                    levelUp = oldUserLevel < newUserLevel,
                    activityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(newUserLevel)
                });
        }
    }
}