[TestFixture]
internal class SharesService_tests : BaseTestHarness
{
    [Test]
    public async Task Should_create_token_for_page()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);
        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));

        await RecycleContainerAndEntityCache();

        var sharesRepository = R<SharesRepository>();

        // Act
        var token = SharesService.GetShareToken(testPage.Id, SharePermission.View, grantedByUser.Id, sharesRepository);

        // Assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token.Length, Is.EqualTo(32)); // GUID without hyphens

        // Verify token exists in database
        var shares = sharesRepository.GetAllEager();
        var shareInfo = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.Token == token);

        Assert.That(shareInfo, Is.Not.Null);
        Assert.That(shareInfo.Permission, Is.EqualTo(SharePermission.View));
        Assert.That(shareInfo.GrantedBy, Is.EqualTo(grantedByUser.Id));

        // Verify token exists in cache
        var cachedShares = EntityCache.GetPageShares(testPage.Id);
        var cachedToken = cachedShares.FirstOrDefault(s => s.Token == token);

        Assert.That(cachedToken, Is.Not.Null);
        Assert.That(cachedToken.Permission, Is.EqualTo(SharePermission.View));
        Assert.That(cachedToken.GrantedBy, Is.EqualTo(grantedByUser.Id));
    }

    [Test]
    public async Task Should_update_existing_token_for_page()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);
        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));

        await RecycleContainerAndEntityCache();

        var sharesRepository = R<SharesRepository>();
        var initialToken = SharesService.GetShareToken(testPage.Id, SharePermission.View, grantedByUser.Id, sharesRepository);

        // Act
        var updatedToken = SharesService.GetShareToken(testPage.Id, SharePermission.Edit, grantedByUser.Id, sharesRepository);

        // Assert
        Assert.That(updatedToken, Is.EqualTo(initialToken), "Token should remain the same after updating permission");

        // Verify permission was updated in DB
        var shares = sharesRepository.GetAllEager();
        var shareInfo = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.Token == updatedToken);

        Assert.That(shareInfo, Is.Not.Null);
        Assert.That(shareInfo.Permission, Is.EqualTo(SharePermission.Edit));

        // Verify cache was updated
        var cachedShares = EntityCache.GetPageShares(testPage.Id);
        var cachedToken = cachedShares.FirstOrDefault(s => s.Token == updatedToken);

        Assert.That(cachedToken, Is.Not.Null);
        Assert.That(cachedToken.Permission, Is.EqualTo(SharePermission.Edit));
    }

    [Test]
    public async Task Should_renew_share_token()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);
        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));

        await RecycleContainerAndEntityCache();

        var sharesRepository = R<SharesRepository>();
        var initialToken = SharesService.GetShareToken(testPage.Id, SharePermission.View, grantedByUser.Id, sharesRepository);

        // Act
        var newToken = SharesService.RenewShareToken(testPage.Id, grantedByUser.Id, sharesRepository);

        // Assert
        Assert.That(newToken, Is.Not.Null);
        Assert.That(newToken, Is.Not.EqualTo(initialToken), "New token should be different from the initial token");

        // Verify new token exists in database
        var shares = sharesRepository.GetAllEager();
        var shareInfo = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.Token == newToken);

        Assert.That(shareInfo, Is.Not.Null);

        // Verify old token no longer exists
        var oldShareInfo = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.Token == initialToken);
        Assert.That(oldShareInfo, Is.Null);

        // Verify cache was updated
        var cachedShares = EntityCache.GetPageShares(testPage.Id);
        var cachedToken = cachedShares.FirstOrDefault(s => s.Token == newToken);

        Assert.That(cachedToken, Is.Not.Null);
    }

    [Test]
    public async Task Should_throw_exception_when_renewing_nonexistent_token()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);
        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));

        await RecycleContainerAndEntityCache();

        var sharesRepository = R<SharesRepository>();

        // Act & Assert
        var ex = Assert.Throws<Exception>(() =>
            SharesService.RenewShareToken(testPage.Id, grantedByUser.Id, sharesRepository));

        Assert.That(ex.Message, Is.EqualTo("Cannot renew ShareToken, missing ShareItem"));
    }

    [Test]
    public async Task Should_remove_share_token()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);
        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));

        await RecycleContainerAndEntityCache();

        var sharesRepository = R<SharesRepository>();
        SharesService.GetShareToken(testPage.Id, SharePermission.View, grantedByUser.Id, sharesRepository);

        // Act
        SharesService.RemoveShareToken(testPage.Id, sharesRepository);

        // Assert
        var shares = sharesRepository.GetAllEager();
        var shareInfo = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.Token.Length > 0);

        Assert.That(shareInfo, Is.Null, "Token share should be removed from the database");

        // Verify cache was updated
        var cachedShares = EntityCache.GetPageShares(testPage.Id);
        var cachedToken = cachedShares.FirstOrDefault(s => s.Token.Length > 0);

        Assert.That(cachedToken, Is.Null, "Token share should be removed from the cache");
    }

    [Test]
    public void Should_add_user_share_to_page()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var userReadingRepo = R<UserReadingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);

        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var testUser = contextUser
            .Add(new User { Name = "TestShareUser", EmailAddress = "test.share.user@example.com" })
            .Persist()
            .GetUser("TestShareUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));

        var sharesRepository = R<SharesRepository>();

        // Act
        SharesService.AddShareToPage(
            testPage.Id,
            testUser.Id,
            SharePermission.Edit,
            grantedByUser.Id,
            sharesRepository,
            userReadingRepo);

        // Assert
        var shares = sharesRepository.GetAllEager();
        var shareInfo = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.User?.Id == testUser.Id);

        Assert.That(shareInfo, Is.Not.Null);
        Assert.That(shareInfo.Permission, Is.EqualTo(SharePermission.Edit));
        Assert.That(shareInfo.GrantedBy, Is.EqualTo(grantedByUser.Id));
        Assert.That(shareInfo.Token, Is.Empty, "User share should have empty token");

        // Verify cache was updated
        var cachedShares = EntityCache.GetPageShares(testPage.Id);
        var cachedShare = cachedShares.FirstOrDefault(s => s.SharedWith?.Id == testUser.Id);

        Assert.That(cachedShare, Is.Not.Null);
        Assert.That(cachedShare.Permission, Is.EqualTo(SharePermission.Edit));
        Assert.That(cachedShare.GrantedBy, Is.EqualTo(grantedByUser.Id));
        Assert.That(cachedShare.Token, Is.Empty);
    }

    [Test]
    public void Should_update_existing_user_share_permission()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var userReadingRepo = R<UserReadingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);

        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var testUser = contextUser
            .Add(new User { Name = "TestShareUser", EmailAddress = "test.share.user@example.com" })
            .Persist()
            .GetUser("TestShareUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));

        var sharesRepository = R<SharesRepository>();

        // Initial share with View permission
        SharesService.AddShareToPage(testPage.Id, testUser.Id, SharePermission.View, grantedByUser.Id,
            sharesRepository, userReadingRepo);

        // Act - Update to EditWithChildren permission
        SharesService.AddShareToPage(testPage.Id, testUser.Id, SharePermission.EditWithChildren, grantedByUser.Id,
            sharesRepository, userReadingRepo);

        // Assert
        var shares = sharesRepository.GetAllEager();
        var shareInfo = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.User?.Id == testUser.Id);

        Assert.That(shareInfo, Is.Not.Null);
        Assert.That(shareInfo.Permission, Is.EqualTo(SharePermission.EditWithChildren));

        // Verify cache was updated
        var cachedShares = EntityCache.GetPageShares(testPage.Id);
        var cachedShare = cachedShares.FirstOrDefault(s => s.SharedWith?.Id == testUser.Id);

        Assert.That(cachedShare, Is.Not.Null);
        Assert.That(cachedShare.Permission, Is.EqualTo(SharePermission.EditWithChildren));
    }

    [Test]
    public void Should_get_closest_parent_share_permission_by_userId()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var userReadingRepo = R<UserReadingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);

        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var testUser = contextUser
            .Add(new User { Name = "TestShareUser", EmailAddress = "test.share.user@example.com" })
            .Persist()
            .GetUser("TestShareUser");

        var contextPage = NewPageContext();
        var parentPage = contextPage
            .Add("ParentPage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("ParentPage"));

        var childPage = contextPage
            .Add("ChildPage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("ChildPage"));

        contextPage.AddChild(parentPage, childPage);

        var sharesRepository = R<SharesRepository>();

        // Add share with child access
        SharesService.AddShareToPage(parentPage.Id, testUser.Id, SharePermission.ViewWithChildren,
            grantedByUser.Id, sharesRepository, userReadingRepo);

        // Act
        var permission = SharesService.GetClosestParentSharePermissionByUserId(childPage.Id, testUser.Id);

        // Assert
        Assert.That(permission, Is.Not.Null);
        Assert.That(permission, Is.EqualTo(SharePermission.ViewWithChildren));
    }

    [Test]
    public void Should_get_closest_parent_share_permission_by_token()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);

        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var contextPage = NewPageContext();
        var parentPage = contextPage
            .Add("ParentPage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("ParentPage"));

        var childPage = contextPage
            .Add("ChildPage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("ChildPage"));

        contextPage.AddChild(parentPage, childPage);

        var sharesRepository = R<SharesRepository>();

        // Create token with child access
        var token = SharesService.GetShareToken(parentPage.Id, SharePermission.EditWithChildren,
            grantedByUser.Id, sharesRepository);

        RecycleContainerAndEntityCache();


        // Act
        var permission = SharesService.GetClosestParentSharePermissionByTokens(childPage.Id, null, token);

        // Assert
        Assert.That(permission, Is.Not.Null);
        Assert.That(permission, Is.EqualTo(SharePermission.EditWithChildren));
    }

    [Test]
    public void Should_determine_if_page_is_shared()
    {
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var userReadingRepo = R<UserReadingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);

        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var testUser = contextUser
            .Add(new User { Name = "TestShareUser", EmailAddress = "test.share.user@example.com" })
            .Persist()
            .GetUser("TestShareUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));

        var sharesRepository = R<SharesRepository>();

        // Act & Assert - initially not shared
        Assert.That(SharesService.IsShared(testPage.Id), Is.False);

        // Add share
        SharesService.AddShareToPage(testPage.Id, testUser.Id, SharePermission.View,
            grantedByUser.Id, sharesRepository, userReadingRepo);


        // Now should be shared
        Assert.That(SharesService.IsShared(testPage.Id), Is.True);
    }

    [Test]
    public void Should_batch_update_page_shares()
    {
        RecycleContainerAndEntityCache();
        // Arrange
        var userWritingRepo = R<UserWritingRepo>();
        var userReadingRepo = R<UserReadingRepo>();
        var contextUser = ContextUser.New(userWritingRepo);

        var grantedByUser = contextUser
            .Add(new User { Name = "GrantingUser", EmailAddress = "granting.user@example.com" })
            .Persist()
            .GetUser("GrantingUser");

        var firstUser = contextUser
            .Add(new User { Name = "FirstUser", EmailAddress = "first.user@example.com" })
            .Persist()
            .GetUser("FirstUser");

        var secondUser = contextUser
            .Add(new User { Name = "SecondUser", EmailAddress = "second.user@example.com" })
            .Persist()
            .GetUser("SecondUser");

        var contextPage = NewPageContext();
        var testPage = contextPage
            .Add("TestSharePage", creator: grantedByUser)
            .Persist()
            .All
            .Single(p => p.Name.Equals("TestSharePage"));


        var sharesRepository = R<SharesRepository>();

        // Create initial shares
        SharesService.AddShareToPage(testPage.Id, firstUser.Id, SharePermission.View,
            grantedByUser.Id, sharesRepository, userReadingRepo);
        SharesService.AddShareToPage(testPage.Id, secondUser.Id, SharePermission.View,
            grantedByUser.Id, sharesRepository, userReadingRepo);
        SharesService.GetShareToken(testPage.Id, SharePermission.View,
            grantedByUser.Id, sharesRepository);

        // Prepare batch update
        var permissionUpdates = new List<(int UserId, SharePermission Permission)>
            {
                (firstUser.Id, SharePermission.EditWithChildren)
            };
        var userIdsToRemove = new List<int> { secondUser.Id };

        // Act
        SharesService.BatchUpdatePageShares(
            testPage.Id,
            permissionUpdates,
            userIdsToRemove,
            true, // Remove share token,
            null,
            null,
            grantedByUser.Id,
            sharesRepository,
            userReadingRepo
        );

        // Assert
        var shares = sharesRepository.GetAllEager();

        // First user's permission should be updated
        var firstUserShare = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.User?.Id == firstUser.Id);
        Assert.That(firstUserShare, Is.Not.Null);
        Assert.That(firstUserShare.Permission, Is.EqualTo(SharePermission.EditWithChildren));

        // Second user's share should be removed
        var secondUserShare = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.User?.Id == secondUser.Id);
        Assert.That(secondUserShare, Is.Null);

        // Token share should be removed
        var tokenShare = shares.FirstOrDefault(s => s.PageId == testPage.Id && s.Token.Length > 0);
        Assert.That(tokenShare, Is.Null);

        // Check cache
        var cachedShares = EntityCache.GetPageShares(testPage.Id);
        Assert.That(cachedShares.Count, Is.EqualTo(1));
        Assert.That(cachedShares[0].SharedWith?.Id, Is.EqualTo(firstUser.Id));
        Assert.That(cachedShares[0].Permission, Is.EqualTo(SharePermission.EditWithChildren));
    }
}

