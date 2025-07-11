﻿// Repositories/ShareInfoRepository.cs
using NHibernate;

public class SharesRepository(ISession session) : RepositoryDbBase<Share>(session)
{
    public IList<Share> GetAllEager()
    {
        var shares = _session.QueryOver<Share>().Future().ToList();

        _session.QueryOver<Share>()
            .Fetch(SelectMode.Fetch, x => x.User)
            .Future();

        var result = shares;

        foreach (var share in result)
        {
            NHibernateUtil.Initialize(share.User);
        }

        return result.ToList();
    }

    public override void Create(Share share)
    {
        base.Create(share);
        Flush();
    }

    public override void Update(Share share)
    {
        base.Update(share);
        Flush();
    }

    public override void Delete(Share share)
    {
        base.Delete(share);
        Flush();
    }

    public void Delete(IList<int> shareIds)
    {
        if (shareIds == null || !shareIds.Any())
            return;

        foreach (var shareId in shareIds)
        {
            var share = GetById(shareId);
            if (share != null)
            {
                base.Delete(share);
            }
        }

        Flush();
    }

    public void DeleteAllForPage(int pageId)
    {
        using var transaction = _session.BeginTransaction();

        try
        {
            _session.CreateQuery("DELETE FROM Share WHERE PageId = :pageId")
                .SetParameter("pageId", pageId)
                .ExecuteUpdate();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            Log.Error($"Error deleting shares for page {pageId}: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Delete all shares for a page without creating its own transaction.
    /// Use this method when calling from within an existing transaction.
    /// </summary>
    public void DeleteAllForPageWithoutTransaction(int pageId)
    {
        _session.CreateQuery("DELETE FROM Share WHERE PageId = :pageId")
            .SetParameter("pageId", pageId)
            .ExecuteUpdate();
    }

    public void DeleteAllForUser(int userId)
    {
        using var transaction = _session.BeginTransaction();

        try
        {
            _session.CreateQuery("DELETE FROM Share WHERE UserId = :userId")
                .SetParameter("userId", userId)
                .ExecuteUpdate();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            Log.Error($"Error deleting shares for user {userId}: {ex.Message}");
            throw;
        }
    }

}