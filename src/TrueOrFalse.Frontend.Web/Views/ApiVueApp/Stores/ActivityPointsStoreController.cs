using Microsoft.AspNetCore.Mvc;
using System;

public class ActivityPointsStoreController(
    SessionUser _sessionUser,
    ActivityPointsRepo _activityPointsRepo,
    UserWritingRepo _userWritingRepo) : Controller
{
    public readonly record struct AddJson(string ActivityTypeString, int Points);

    public readonly record struct AddResult(
        int Points,
        int Level,
        bool LevelUp,
        int ActivityPointsTillNextLevel,
        int ActivityPointsPercentageOfNextLevel);

    [HttpPost]
    public AddResult Add([FromBody] AddJson activityPointsData)
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
            var activityPointsTillNextLevel =
                activityPointsAtNextLevel - _sessionUser.User.ActivityPoints;
            var activityPointsPercentageOfNextLevel = _sessionUser.User.ActivityPoints == 0
                ? 0
                : 100 * _sessionUser.User.ActivityPoints / activityPointsAtNextLevel;

            return new AddResult
            {
                Points = _sessionUser.User.ActivityPoints,
                Level = activityLevel,
                LevelUp = oldUserLevel < activityLevel,
                ActivityPointsTillNextLevel = activityPointsTillNextLevel,
                ActivityPointsPercentageOfNextLevel = activityPointsPercentageOfNextLevel
            };
        }
        else
        {
            var oldUserLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());
            _sessionUser.AddPointActivity(activityPoints);
            var newUserLevel = UserLevelCalculator.GetLevel(_sessionUser.GetTotalActivityPoints());

            return new AddResult
            {
                Points = _sessionUser.GetTotalActivityPoints(),
                Level = newUserLevel,
                LevelUp = oldUserLevel < newUserLevel,
                ActivityPointsTillNextLevel = UserLevelCalculator.GetUpperLevelBound(newUserLevel)
            };
        }
    }
}