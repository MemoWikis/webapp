﻿using Seedworks.Lib.Persistence;
using System.Diagnostics;
using TrueOrFalse.Search;
using ISession = NHibernate.ISession;

public class UserWritingRepo
{
    private readonly SessionUser _sessionUser;

    private readonly ActivityPointsRepo _activityPointsRepo;
    private readonly UserReadingRepo _userReadingRepo;
    private readonly ReputationCalc _reputationCalc;
    private readonly GetWishQuestionCount _getWishQuestionCount;
    private readonly ExtendedUserCache _extendedUserCache;
    private readonly RepositoryDb<User> _repo;

    public UserWritingRepo(
        ISession session,
        SessionUser sessionUser,
        ActivityPointsRepo activityPointsRepo,
        UserReadingRepo userReadingRepo,
        ReputationCalc reputationCalc,
        GetWishQuestionCount getWishQuestionCount,
        ExtendedUserCache extendedUserCache)
    {
        _repo = new RepositoryDb<User>(session);
        _sessionUser = sessionUser;
        _activityPointsRepo = activityPointsRepo;
        _userReadingRepo = userReadingRepo;
        _reputationCalc = reputationCalc;
        _getWishQuestionCount = getWishQuestionCount;
        _extendedUserCache = extendedUserCache;
    }

    public void ApplyChangeAndUpdate(int userId, Action<User> change)
    {
        var user = _repo.GetById(userId);
        change(user);
        Update(user);
    }

    public void Create(User user)
    {
        Logg.r.Information("user create {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress,
            new StackTrace());

        _repo.Create(user);
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
        _extendedUserCache.Remove(user);
        EntityCache.RemoveUser(id);
        Task.Run(async () =>
            await new MeiliSearchUsersDatabaseOperations()
                .DeleteAsync(user));
    }

    public void DeleteFromAllTables(int userId)
    {
        var user = _repo.GetById(userId);
        Task.Run(async () => await new MeiliSearchUsersDatabaseOperations().DeleteAsync(user));

        Logg.r.Information($"Starting deletion of user {userId} and related data.");

        using var transaction = _repo.Session.BeginTransaction();

        try
        {
            _repo.Session.CreateSQLQuery("DELETE FROM persistentlogin WHERE UserId = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            _repo.Session.CreateSQLQuery("DELETE FROM activitypoints WHERE User_Id = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            _repo.Session.CreateSQLQuery("DELETE FROM messageemail WHERE User_Id = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("Update questionValuation SET Userid = null WHERE UserId = :userId")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("Update pagevaluation SET Userid = null WHERE UserId = :userId")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("UPDATE learningSession SET User_Id = null WHERE User_id = :userId")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("UPDATE page SET Creator_Id = null WHERE Creator_id = :userId")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("UPDATE pageview SET User_Id = null WHERE User_id = :userId")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session.CreateSQLQuery("DELETE FROM answer WHERE UserId = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("Update imagemetadata Set userid  = null Where userid =  :userId")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("Update comment Set Creator_id  = null Where Creator_id = :userId")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session.CreateSQLQuery("DELETE FROM answer WHERE UserId = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("Update questionview  Set UserId = null Where UserId = :userId")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session.CreateSQLQuery("UPDATE pagechange c " +
                                         "JOIN user u ON u.id = c.author_id Set c.author_id = null " +
                                         "WHERE u.id =  :userid;")
                .SetParameter("userid", userId)
                .ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery(
                    "Update questionchange qc set qc.Author_id = null Where Author_id = :userid")
                .SetParameter("userid", userId)
                .ExecuteUpdate();

            _repo.Session.CreateSQLQuery(
                    "DELETE uf.* From  user u LEFT JOIN user_to_follower uf ON u.id = uf.user_id Where u.id = :userid")
                .SetParameter("userid", userId)
                .ExecuteUpdate();

            _repo.Session.CreateSQLQuery(
                    "DELETE uf.* From  user u LEFT JOIN user_to_follower uf ON u.id = uf.Follower_id Where u.id = :userid")
                .SetParameter("userid", userId)
                .ExecuteUpdate();

            _repo.Session.CreateSQLQuery(
                    "Delete ua.* From Useractivity ua  Join question q ON ua.question_id = q.id where q.creator_id = :userid and (visibility = 1 Or visibility = 2)")
                .SetParameter("userid", userId)
                .ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("Delete From question where creator_id = :userid and visibility = 1")
                .SetParameter("userid", userId)
                .ExecuteUpdate();

            _repo.Session
                .CreateSQLQuery("Update question  Set Creator_Id = null Where Creator_Id = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            _repo.Session.CreateSQLQuery(
                    "Delete ua.* From useractivity ua Left Join  user u ON u.id = ua.UserConcerned_id Where u.id  =  :userId;")
                .SetParameter("userId", userId).ExecuteUpdate();

            _repo.Session.CreateSQLQuery(
                    "Delete ua.* From useractivity ua Left Join  user u ON u.id = ua.UserISFollowed_id Where u.id  =  :userId;")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            _repo.Session.CreateSQLQuery("DELETE FROM user WHERE id = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            transaction.Commit();
            Logg.r.Information($"Successfully deleted user {userId} and related data.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Logg.r.Error($"Error deleting user {userId}: {ex.Message}", ex);
            throw;
        }
    }

    public void Update(User user)
    {
        Logg.r.Information("user update {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress,
            new StackTrace());

        _repo.Update(user);
        _extendedUserCache.Update(user);
        EntityCache.AddOrUpdate(UserCacheItem.ToCacheUser(user));
        Task.Run(async () =>
            await new MeiliSearchUsersDatabaseOperations()
                .UpdateAsync(user));
    }

    public void Update(UserCacheItem userCacheItem)
    {
        var user = _repo.GetById(userCacheItem.Id);
        if (user != null)
        {
            user.EmailAddress = userCacheItem.EmailAddress;
            user.Name = userCacheItem.Name;
            user.FacebookId = userCacheItem.FacebookId;
            user.GoogleId = userCacheItem.GoogleId;
            user.Reputation = userCacheItem.Reputation;
            user.ReputationPos = userCacheItem.ReputationPos;
            user.FollowerCount = userCacheItem.FollowerCount;
            user.ShowWishKnowledge = userCacheItem.ShowWishKnowledge;
            user.WikiIds = string.Join(",", userCacheItem.WikiIds.Distinct());
            user.FavoriteIds = string.Join(",", userCacheItem.FavoriteIds.Distinct());
            user.UiLanguage = userCacheItem.UiLanguage;

            Update(user);
        }
    }

    public void UpdateActivityPointsData()
    {
        if (!_sessionUser.IsLoggedIn)
        {
            return;
        }

        var totalPointCount = 0;
        foreach (var activityPoints in _activityPointsRepo.GetActivtyPointsByUser(_sessionUser
                     .UserId))
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
        var newReputation = userToUpdate.Reputation =
            _reputationCalc.Run(userToUpdateCacheItem).TotalReputation;

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
        Logg.r.Information("user update {Id} {Email} {Stacktrace}", user.Id, user.EmailAddress,
            new StackTrace());
        Update(user);
    }
}