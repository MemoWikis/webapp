﻿public class SharesService
{
    public static string GetShareToken(int pageId, SharePermission permission, int grantedById, SharesRepository sharesRepository)
    {
        var token = Guid.NewGuid().ToString("N");
        var existingShares = EntityCache.GetPageShares(pageId);

        var currentShareByToken = existingShares.FirstOrDefault(share => share.Token.Length > 0);
        var shareInfo = new Share();
        if (currentShareByToken != null)
        {
            currentShareByToken.Permission = permission;
            currentShareByToken.GrantedBy = grantedById;

            EntityCache.AddOrUpdate(currentShareByToken);

            var dbItem = sharesRepository.GetById(currentShareByToken.Id);
            dbItem.Permission = permission;
            dbItem.GrantedBy = grantedById;
            sharesRepository.Update(dbItem);
            return currentShareByToken.Token;
        }
        else
        {
            shareInfo = new Share
            {
                User = null,
                PageId = pageId,
                Permission = permission,
                GrantedBy = grantedById,
                Token = token
            };

            sharesRepository.CreateOrUpdate(shareInfo);
            var dbItem = sharesRepository.GetById(shareInfo.Id);
            var newShareInfoCacheItem = ShareCacheItem.ToCacheItem(dbItem);
            existingShares.Add(newShareInfoCacheItem);
            return shareInfo.Token;

        }
    }

    public static string RenewShareToken(int pageId, int grantedById, SharesRepository sharesRepository)
    {
        var token = Guid.NewGuid().ToString("N");
        var existingShares = EntityCache.GetPageShares(pageId);

        var currentShareByToken = existingShares.FirstOrDefault(share => share.Token.Length > 0);

        if (currentShareByToken == null)
            throw new Exception("Cannot renew ShareToken, missing ShareItem");

        currentShareByToken.GrantedBy = grantedById;
        currentShareByToken.Token = token;

        EntityCache.AddOrUpdate(currentShareByToken);

        var dbItem = sharesRepository.GetById(currentShareByToken.Id);
        dbItem.GrantedBy = grantedById;
        dbItem.Token = token;
        sharesRepository.Update(dbItem);
        return currentShareByToken.Token;

    }

    public static void AddShareToPage(int pageId, int userId, SharePermission permission, int grantedById, SharesRepository sharesRepository, UserReadingRepo userReadingRepo)
    {
        var existingShares = EntityCache.GetPageShares(pageId);
        var existingShare = existingShares.FirstOrDefault(s => s.SharedWith?.Id == userId);
        if (existingShare != null)
        {
            existingShare.Permission = permission;
            sharesRepository.CreateOrUpdate(existingShare.ToDbItem(userReadingRepo));
        }
        else
        {
            var shareInfo = new Share
            {
                User = userReadingRepo.GetById(userId),
                PageId = pageId,
                Permission = permission,
                GrantedBy = grantedById,
                Token = ""
            };

            sharesRepository.CreateOrUpdate(shareInfo);
            var dbItem = sharesRepository.GetById(shareInfo.Id);
            var newShareInfoCacheItem = ShareCacheItem.ToCacheItem(dbItem);
            existingShares.Add(newShareInfoCacheItem);
        }

        EntityCache.AddOrUpdatePageShares(pageId, existingShares);
    }

    public static SharePermission? GetClosestParentSharePermission(int childId, int? userId, string? token = null)
    {
        var child = EntityCache.GetPage(childId);
        if (child == null)
            return null;

        var visited = new HashSet<int> { childId };
        var currentGeneration = new List<int>(child.ParentRelations.Select(r => r.ParentId));

        while (currentGeneration.Count > 0)
        {
            currentGeneration = currentGeneration.Where(pid => !visited.Contains(pid)).ToList();
            if (currentGeneration.Count == 0) break;

            foreach (var pid in currentGeneration)
                visited.Add(pid);

            SharePermission? bestPermissionThisLevel = null;

            foreach (var pid in currentGeneration)
            {
                var parent = EntityCache.GetPage(pid);
                if (parent == null) continue;
                if (userId != null)
                {
                    var shareInfo = parent.GetDirectShareInfos().FirstOrDefault(s => s.SharedWith?.Id == userId);
                    if (shareInfo != null)
                    {
                        bestPermissionThisLevel = GetHigherPermission(bestPermissionThisLevel, shareInfo.Permission);
                    }
                }

                else if (token != null)
                {
                    var shareInfo = parent.GetDirectShareInfos().FirstOrDefault(s => s.Token == token);
                    if (shareInfo != null)
                    {
                        bestPermissionThisLevel = GetHigherPermission(bestPermissionThisLevel, shareInfo.Permission);
                    }
                }

            }

            if (bestPermissionThisLevel.HasValue)
                return bestPermissionThisLevel;

            var nextGeneration = new List<int>();
            foreach (var pid in currentGeneration)
            {
                var p = EntityCache.GetPage(pid);
                if (p != null)
                {
                    nextGeneration.AddRange(p.ParentRelations.Select(r => r.ParentId));
                }
            }

            currentGeneration = nextGeneration;
        }

        return null;
    }

    private static SharePermission? GetHigherPermission(SharePermission? current, SharePermission candidate)
    {
        if (!current.HasValue)
            return candidate;

        var rankCurrent = GetRank(current.Value);
        var rankCandidate = GetRank(candidate);
        return rankCandidate > rankCurrent ? candidate : current;
    }

    private static int GetRank(SharePermission perm)
    {
        return perm switch
        {
            SharePermission.Edit => 1,
            SharePermission.View => 2,
            SharePermission.EditWithChildren => 3,
            SharePermission.ViewWithChildren => 4,
            _ => 0
        };
    }
}
