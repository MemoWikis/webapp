using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;

class UserActivityUpdate
{   
    public static void NewFollower(User userFollower, User userIsFollowed)
    {
        //add UserActivity for userIsFollowed, userFollower is the UserConcerned
        
        //var userActivities = new IList<UserActivity>;
        //userActivities.Add(GetCreatedQuestions(userIsFollowed,userFollower)); //doesn't work; either do "((List<myType>)myIList).AddRange(anotherList);" 
        //or change GetCreatedQuestions to AddCreatedQuestions(ref IList<UserActivity> userActivities, ...)
    }

    public static IList<UserActivity> GetCreatedQuestions(User user, User userFollower, int amount=10)
    {
        //Sl.R<ISession>().QueryOver<Question>()... SO GEHT's!
        //var questions = _session.QueryOver<Question>
        //    .OrderBy(x => x.DateCreated).Desc
        //    .Where(q => q.Visibility == QuestionVisibility.All || q.Creator.Id == currentUser)
        //    .Take(amount)
        //    .List<Question>();

        //var userActivities = new IList<UserActivity>;
        //foreach (var question in questions)
        //{
        //    userActivities.Add(new UserActivity {
        //            UserConcerned = userFollower,
        //            At = question.DateCreated,
        //            Type = UserActivityType.CreatedQuestion,
        //            Question = question,
        //            UserCauser = question.Creator
        //        });
        //}
        //return userActivities;
        return null;
    }
}

