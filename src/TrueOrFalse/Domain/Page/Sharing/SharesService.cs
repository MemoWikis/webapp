public class SharesService
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

            EntityCache.AddOrUpdate(newShareInfoCacheItem);

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

    public static void RemoveShareToken(int pageId, SharesRepository sharesRepository)
    {
        var existingShares = EntityCache.GetPageShares(pageId);
        var shareIdsToRemove = existingShares.Where(share => share.Token.Length > 0).Select(share => share.Id).ToList();
        EntityCache.RemoveShares(pageId, shareIdsToRemove);
        sharesRepository.Delete(shareIdsToRemove);
    }

    public static void AddShareToPage(int pageId, int userId, SharePermission permission, int grantedById, SharesRepository sharesRepository, UserReadingRepo userReadingRepo)
    {
        var existingShares = EntityCache.GetPageShares(pageId);
        var existingShare = existingShares.FirstOrDefault(s => s.SharedWith?.Id == userId);
        if (existingShare != null)
        {
            existingShare.Permission = permission;
            var dbShare = sharesRepository.GetById(existingShare.Id);
            if (dbShare != null)
            {
                dbShare.Permission = existingShare.Permission;
                sharesRepository.Update(dbShare);
            }
        }
        else
        {
            var user = userReadingRepo.GetById(userId);
            var shareInfo = new Share
            {
                User = user,
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

    public static SharePermission? GetClosestParentSharePermissionByUserId(int childId, int? userId)
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
                    var shareInfo = parent.GetDirectShares().FirstOrDefault(s => s.SharedWith?.Id == userId);
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

    public static SharePermission? GetClosestParentSharePermissionByTokens(int childId, Dictionary<int, string>? pageTokens, string? providedToken = null)
    {
        if (pageTokens == null || !pageTokens.Any())
            return null;

        var child = EntityCache.GetPage(childId);
        if (child == null)
            return null;

        if (pageTokens.TryGetValue(childId, out var directToken))
        {
            var directShares = child.GetDirectShares();
            var directShareByToken = directShares.FirstOrDefault(s => s.Token == directToken);
            if (directShareByToken != null)
                return directShareByToken.Permission;
        }

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

                if (!String.IsNullOrEmpty(providedToken))
                {
                    var share = parent.GetDirectShares().FirstOrDefault(s => s.Token == providedToken);
                    if (share != null)
                    {
                        bestPermissionThisLevel = GetHigherPermission(bestPermissionThisLevel, share.Permission);

                        if (bestPermissionThisLevel is SharePermission.ViewWithChildren or SharePermission.EditWithChildren)
                            return bestPermissionThisLevel;
                    }
                }

                else if (pageTokens.TryGetValue(pid, out var token))
                {
                    var shareInfo = parent.GetDirectShares().FirstOrDefault(s => s.Token == token);
                    if (shareInfo != null)
                    {
                        bestPermissionThisLevel = GetHigherPermission(bestPermissionThisLevel, shareInfo.Permission);

                        if (bestPermissionThisLevel is SharePermission.ViewWithChildren or SharePermission.EditWithChildren)
                            return bestPermissionThisLevel;
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
            SharePermission.RestrictAccess => 10,
            _ => 0
        };
    }

    public static Dictionary<int, List<SharePermission>> GetParentSharePermissionsWithChildAccess(int childId)
    {
        var result = new Dictionary<int, List<SharePermission>>();
        var child = EntityCache.GetPage(childId);
        if (child == null)
            return result;

        var visited = new HashSet<int> { childId };
        var currentGeneration = new List<int>(child.ParentRelations.Select(r => r.ParentId));

        while (currentGeneration.Count > 0)
        {
            currentGeneration = currentGeneration.Where(pid => !visited.Contains(pid)).ToList();
            if (currentGeneration.Count == 0) break;

            foreach (var pid in currentGeneration)
                visited.Add(pid);

            foreach (var pid in currentGeneration)
            {
                var parent = EntityCache.GetPage(pid);
                if (parent == null) continue;

                var directShareInfos = parent.GetDirectShares();
                if (directShareInfos != null && directShareInfos.Any())
                {
                    var childAccessPermissions = directShareInfos
                        .Where(s => s.Permission == SharePermission.ViewWithChildren ||
                                    s.Permission == SharePermission.EditWithChildren)
                        .Select(s => s.Permission)
                        .Distinct()
                        .ToList();

                    if (childAccessPermissions.Any())
                    {
                        result[pid] = childAccessPermissions;
                    }
                }
            }

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

        return result;
    }

    public static bool IsShared(int pageId)
    {
        return EntityCache.GetPageShares(pageId).Any() || GetParentSharePermissionsWithChildAccess(pageId).Any();
    }

    public static void BatchUpdatePageShares(
        int pageId,
        List<(int UserId, SharePermission Permission)> permissionUpdates,
        List<int> userIdsToRemove,
        bool removeShareToken,
        SharePermission? tokenPermission,
        int grantedById,
        SharesRepository sharesRepository,
        UserReadingRepo userReadingRepo)
    {
        var existingShares = EntityCache.GetPageShares(pageId);

        foreach (var update in permissionUpdates)
        {
            var share = existingShares.FirstOrDefault(s => s.SharedWith?.Id == update.UserId);
            if (share != null)
            {
                share.Permission = update.Permission;
                var dbShare = sharesRepository.GetById(share.Id);
                if (dbShare != null)
                {
                    dbShare.Permission = update.Permission;
                    sharesRepository.Update(dbShare);
                }
            }
            else
            {
                var newShare = new Share
                {
                    User = userReadingRepo.GetById(update.UserId),
                    PageId = pageId,
                    Permission = update.Permission,
                    GrantedBy = grantedById,
                    Token = ""
                };
                sharesRepository.CreateOrUpdate(newShare);
                var dbItem = sharesRepository.GetById(newShare.Id);
                var newShareCacheItem = ShareCacheItem.ToCacheItem(dbItem);
                existingShares.Add(newShareCacheItem);
            }
        }

        foreach (var userId in userIdsToRemove)
        {
            var share = existingShares.FirstOrDefault(s => s.SharedWith?.Id == userId);
            if (share != null)
            {
                sharesRepository.Delete(share.Id);
            }
        }
        existingShares.RemoveAll(s => s.SharedWith != null && userIdsToRemove.Contains(s.SharedWith.Id));
        EntityCache.AddOrUpdatePageShares(pageId, existingShares);

        if (removeShareToken)
        {
            RemoveShareToken(pageId, sharesRepository);
        }
        else if (tokenPermission.HasValue)
        {
            var tokenShare = existingShares.FirstOrDefault(s => !string.IsNullOrEmpty(s.Token));

            if (tokenShare == null)
            {
                // Create new token with the specified permission
                GetShareToken(pageId, tokenPermission.Value, grantedById, sharesRepository);
            }
            else
            {
                // Update existing token permission
                tokenShare.Permission = tokenPermission.Value;
                EntityCache.AddOrUpdate(tokenShare);

                var dbItem = sharesRepository.GetById(tokenShare.Id);
                if (dbItem != null)
                {
                    dbItem.Permission = tokenPermission.Value;
                    sharesRepository.Update(dbItem);
                }
            }
        }
    }

    public static List<ShareCacheItem> GetParentShareCacheItem(int childId)
    {
        var result = new Dictionary<int, ShareCacheItem>(); // UserId -> ShareCacheItem
        var tokenShares = new List<ShareCacheItem>(); // Shares with tokens (no UserId)

        var child = EntityCache.GetPage(childId);
        if (child == null)
            return new List<ShareCacheItem>();

        var visited = new HashSet<int> { childId };
        var currentGeneration = new List<int>(child.ParentRelations.Select(r => r.ParentId));

        while (currentGeneration.Count > 0)
        {
            currentGeneration = currentGeneration.Where(pid => !visited.Contains(pid)).ToList();
            if (currentGeneration.Count == 0) break;

            foreach (var pid in currentGeneration)
                visited.Add(pid);

            foreach (var pid in currentGeneration)
            {
                var parent = EntityCache.GetPage(pid);
                if (parent == null) continue;

                var directShares = parent.GetDirectShares();
                if (directShares != null && directShares.Any())
                {
                    foreach (var share in directShares)
                    {
                        if (share.Permission == SharePermission.ViewWithChildren ||
                            share.Permission == SharePermission.EditWithChildren)
                        {
                            if (share.SharedWith != null)
                            {
                                if (!result.ContainsKey(share.SharedWith.Id))
                                {
                                    result.Add(share.SharedWith.Id, share);
                                }
                            }
                            else if (!string.IsNullOrEmpty(share.Token))
                            {
                                tokenShares.Add(share);
                            }
                        }
                    }
                }
            }

            if (result.Any() || tokenShares.Any())
                break;

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

        var allShares = result.Values.ToList();
        allShares.AddRange(tokenShares);

        return allShares;
    }

    public static void RemoveAllSharesForPage(int pageId, SharesRepository sharesRepository)
    {
        var existingShares = EntityCache.GetPageShares(pageId);
        var shareIdsToRemove = existingShares.Select(share => share.Id).ToList();
        EntityCache.RemoveShares(pageId, shareIdsToRemove);
        sharesRepository.Delete(shareIdsToRemove);
    }
}
