using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;


public class UserActivityTools
{

    /// <summary>
    /// returns readable version of UserActivityType, e.g. "erstellte die Frage" if question was created
    /// </summary>
    /// <param name="userActivity"></param>
    /// <returns></returns>
    public static string GetActionDescription(UserActivity userActivity)
    {
        switch (userActivity.Type)
        {
            case UserActivityType.CreatedQuestion:
                return "erstellte die Frage";
            case UserActivityType.CreatedCategory:
                return "erstellte das Thema";
            case UserActivityType.CreatedDate:
                return "erstellte den Termin";
            case UserActivityType.CopiedDate:
                return "übernahm den Termin";
            case UserActivityType.CreatedSet:
                return "erstellte das Lernset";
            case UserActivityType.CreatedGame:
                return "erstellte ein Spiel."; //Game has no title
            case UserActivityType.GamePlayed:
                return "spielte ein Quiz.";
            case UserActivityType.FollowedUser:
                return "folgt jetzt";
        }
        return "";
    }

    /// <summary>
    /// returns Html-Code of the activity-object, e.g. the set name if set was created
    /// </summary>
    /// <param name="userActivity"></param>
    /// <returns></returns>
    public static string GetActionObject(UserActivity userActivity)
    {
        switch (userActivity.Type)
        {
            case UserActivityType.CreatedQuestion:
                return string.Format("<a href='{0}'>{1}</a>", Links.AnswerQuestion(userActivity.Question), userActivity.Question.Text);
            case UserActivityType.CreatedCategory:
                return string.Format("<a href='{0}'><span class='label label-category'>{1}</span></a>", Links.CategoryDetail(userActivity.Category), userActivity.Category.Name);
            case UserActivityType.CreatedDate:
                return "<a href='/Termine'>" + userActivity.Date.GetTitle() + "</a>";
            case UserActivityType.CopiedDate:
                return "<a href='/Termine'>" + userActivity.Date.GetTitle() + "</a>";
            case UserActivityType.CreatedSet:
                return string.Format("<a href='{0}'><span class='label label-set'>{1}</span></a>", Links.SetDetail(userActivity.Set.Name, userActivity.Set.Id), userActivity.Set.Name);
            case UserActivityType.CreatedGame:
                return ""; //Game has no title
            case UserActivityType.FollowedUser:
                return string.Format("<a href='{0}'>{1}</a>", Links.UserDetail(userActivity.UserIsFollowed),userActivity.UserIsFollowed.Name);
        }
        return "";
        
    }
}
