﻿using System.Diagnostics;
using Seedworks.Lib.Persistence;
using TrueOrFalse.Search;
using ISession = NHibernate.ISession;

public class UserWritingRepo
{
    private readonly SessionUser _sessionUser;

    private readonly ActivityPointsRepo _activityPointsRepo;
    private readonly ReputationUpdate _reputationUpdate;
    private readonly UserActivityRepo _userActivityRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly ReputationCalc _reputationCalc;
    private readonly GetWishQuestionCount _getWishQuestionCount;
    private readonly SessionUserCache _sessionUserCache;
    private readonly RepositoryDb<User> _repo;


    public UserWritingRepo(ISession session,
        SessionUser sessionUser,
        ActivityPointsRepo activityPointsRepo,
        ReputationUpdate reputationUpdate,
        UserActivityRepo userActivityRepo,
        UserReadingRepo userReadingRepo,
        ReputationCalc reputationCalc,
        GetWishQuestionCount getWishQuestionCount,
        SessionUserCache sessionUserCache)
    {
        _repo = new RepositoryDb<User>(session);
        _sessionUser = sessionUser;
        _activityPointsRepo = activityPointsRepo;
        _reputationUpdate = reputationUpdate;
        _userActivityRepo = userActivityRepo;
        _userReadingRepo = userReadingRepo;
        _reputationCalc = reputationCalc;
        _getWishQuestionCount = getWishQuestionCount;
        _sessionUserCache = sessionUserCache;
    }

    public void ApplyChangeAndUpdate(int userId, Action<User> change)
    {
        var user = _repo.GetById(userId);
        change(user);
        Update(user);
    }

    public virtual void AddFollower(User follower, User user)
    {
        user.Followers.Add(new FollowerInfo
        { Follower = follower, User = user, DateCreated = DateTime.Now, DateModified = DateTime.Now });
        _repo.Flush();
        UserActivityAdd.FollowedUser(follower, user, _userActivityRepo);
        UserActivityUpdate.NewFollower(follower, user, _userActivityRepo, _repo.Session, _userReadingRepo);
        _reputationUpdate.ForUser(user);
    }


    public void Create(User user)
    {
        Logg.r.Information("user create {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress, new StackTrace());

        _repo.Create(user);
        _sessionUserCache.AddOrUpdate(user);

        EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(user));
        Task.Run(async () => await new MeiliSearchUsersDatabaseOperations().CreateAsync(user));
    }

    public void Delete(int id)
    {
        var user = _repo.GetById(id);

        if (_sessionUser.IsLoggedInUserOrAdmin())
        {
            throw new InvalidAccessException();
        }

        _repo.Delete(id);
        _sessionUserCache.Remove(user);
        EntityCache.RemoveUser(id);
        Task.Run(async () => 
            await new MeiliSearchUsersDatabaseOperations()
                .DeleteAsync(user));
    }

    public void DeleteFromAllTables(int userId)
    {
        var user = _repo.Session.Get<User>(userId);
        Task.Run(async () => await new MeiliSearchUsersDatabaseOperations().DeleteAsync(user));

        _repo.Session.CreateSQLQuery("DELETE FROM persistentlogin WHERE UserId = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        _repo.Session.CreateSQLQuery("DELETE FROM activitypoints WHERE User_Id = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        _repo.Session.CreateSQLQuery("DELETE FROM messageemail WHERE User_Id = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        _repo.Session.CreateSQLQuery("Update questionValuation SET Userid = null WHERE UserId = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("Update categoryValuation SET Userid = null WHERE UserId = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("UPDATE learningSession SET User_Id = null WHERE User_id = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("UPDATE category SET Creator_Id = null WHERE Creator_id = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("UPDATE categoryview SET User_Id = null WHERE User_id = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("DELETE FROM answer WHERE UserId = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        _repo.Session.CreateSQLQuery("Update imagemetadata Set userid  = null Where userid =  :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("Update comment Set Creator_id  = null Where Creator_id = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("DELETE FROM answer WHERE UserId = :userId").SetParameter("userId", userId)
            .ExecuteUpdate();
        _repo.Session.CreateSQLQuery("Update questionview  Set UserId = null Where UserId = :userId")
            .SetParameter("userId", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("UPDATE categoryChange c " +
                               "JOIN user u ON u.id = c.author_id Set c.author_id = null " +
                               "WHERE u.id =  :userid;")
            .SetParameter("userid", userId).ExecuteUpdate();

        _repo.Session.CreateSQLQuery("Update questionchange qc set qc.Author_id = null Where Author_id = :userid")
            .SetParameter("userid", userId).ExecuteUpdate();

        _repo.Session.CreateSQLQuery(
            "DELETE uf.* From  user u LEFT JOIN user_to_follower uf ON u.id = uf.user_id Where u.id = :userid")
            .SetParameter("userid", userId).ExecuteUpdate();

        _repo.Session.CreateSQLQuery(
            "DELETE uf.* From  user u LEFT JOIN user_to_follower uf ON u.id = uf.Follower_id Where u.id = :userid")
            .SetParameter("userid", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery(
                "Delete ua.* From Useractivity ua  Join question q ON ua.question_id = q.id where q.creator_id = :userid and (visibility = 1 Or visibility = 2)")
            .SetParameter("userid", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("Delete From question where creator_id = :userid and visibility = 1")
            .SetParameter("userid", userId).ExecuteUpdate();
        _repo.Session.CreateSQLQuery("Update question  Set Creator_Id = null Where Creator_Id = :userId")
            .SetParameter("userId", userId)
            .ExecuteUpdate(); // visibility not necessary because everything has already been deleted

        _repo.Session.CreateSQLQuery(
                "Delete ua.* From useractivity ua Left Join  user u ON u.id = ua.UserConcerned_id Where u.id  =  :userId;")
            .SetParameter("userId", userId).ExecuteUpdate();

        _repo.Session.CreateSQLQuery(
                "Delete ua.* From useractivity ua Left Join  user u ON u.id = ua.UserISFollowed_id Where u.id  =  :userId;")
            .SetParameter("userId", userId).ExecuteUpdate();

        _repo.Session.CreateSQLQuery("Delete From user Where id =  :userId;").SetParameter("userId", userId).ExecuteUpdate();
    }


    public void Update(User user)
    {
        Logg.r.Information("user update {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress, new StackTrace());

        _repo.Update(user);
        _sessionUserCache.AddOrUpdate(user);
        EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(user));
        Task.Run(async () => 
            await new MeiliSearchUsersDatabaseOperations()
                .UpdateAsync(user));
    }

    public void Update(UserCacheItem userCacheItem)
    {
        var user = _repo.GetById(userCacheItem.Id);

        user.EmailAddress = userCacheItem.EmailAddress;
        user.Name = userCacheItem.Name;
        user.FacebookId = userCacheItem.FacebookId;
        user.GoogleId = userCacheItem.GoogleId;
        user.Reputation = userCacheItem.Reputation;
        user.ReputationPos = userCacheItem.ReputationPos;
        user.FollowerCount = userCacheItem.FollowerCount;
        user.ShowWishKnowledge = userCacheItem.ShowWishKnowledge;

        Update(user);
    }

    public void UpdateActivityPointsData()
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return;
        }

        var totalPointCount = 0;
        foreach (var activityPoints in _activityPointsRepo.GetActivtyPointsByUser(_sessionUser.UserId))
        {
            totalPointCount += activityPoints.Amount;
        }

        var userLevel = UserLevelCalculator.GetLevel(totalPointCount);

        var user = _repo.GetById(_sessionUser.UserId);
        user.ActivityPoints = totalPointCount;
        user.ActivityLevel = userLevel;
        Update(user);
    }

    public void ReputationUpdate(User userToUpdate)
    {
        var userToUpdateCacheItem = EntityCache.GetUserById(userToUpdate.Id);

        var oldReputation = userToUpdate.Reputation;
        var newReputation = userToUpdate.Reputation = _reputationCalc.Run(userToUpdateCacheItem).TotalReputation;

        var users = _userReadingRepo.GetWhereReputationIsBetween(newReputation, oldReputation);
        foreach (User user in users)
        {
            userToUpdate.ReputationPos = user.ReputationPos;
            if (newReputation < oldReputation)
                user.ReputationPos--;
            else
                user.ReputationPos++;

            Update(user);
        }

        Update(userToUpdate);
    }

    public void ReputationUpdateForAll()
    {
        var allUsers = UserCacheItem.ToCacheUsers(_userReadingRepo.GetAll());

        var results = allUsers
            .Select(user => _reputationCalc.Run(user))
            .OrderByDescending(r => r.TotalReputation);

        var i = 0;
        foreach (var result in results)
        {
            i++;
            result.User.User.ReputationPos = i;
            result.User.User.Reputation = result.TotalReputation;
            result.User.User.WishCountQuestions = _getWishQuestionCount.Run(result.User.Id);

            Update(result.User.User);
        }
    }

    public void UpdateOnlyDb(User user)
    {
        Logg.r.Information("user update {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress, new StackTrace());
        Update(user);
    }
}