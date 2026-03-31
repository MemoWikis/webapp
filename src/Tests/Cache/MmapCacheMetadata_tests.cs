[TestFixture]
internal class MmapCacheMetadata_tests : BaseTestHarness
{
    public record struct TestViews(DateTime DateTimeNow, int Count);

    [Test]
    public void Save_and_Load_writes_correct_metadata()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            MmapCacheMetadata.Save(tempFile, 42, 1);

            var metadata = MmapCacheMetadata.Load(tempFile);

            Assert.That(metadata, Is.Not.Null);
            Assert.That(metadata!.EntryCount, Is.EqualTo(42));
            Assert.That(metadata.SchemaVersion, Is.EqualTo(1));
            Assert.That(metadata.SavedAtUtc, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromSeconds(5)));
        }
        finally
        {
            File.Delete(tempFile);
            MmapCacheMetadata.Delete(tempFile);
        }
    }

    [Test]
    public void Validate_returns_null_for_valid_cache()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            MmapCacheMetadata.Save(tempFile, 100, 1);

            var rejection = MmapCacheMetadata.Validate(tempFile, expectedSchemaVersion: 1, loadedEntryCount: 100);

            Assert.That(rejection, Is.Null);
        }
        finally
        {
            File.Delete(tempFile);
            MmapCacheMetadata.Delete(tempFile);
        }
    }

    [Test]
    public void Validate_rejects_missing_metadata_file()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".mmap");

        var rejection = MmapCacheMetadata.Validate(tempFile, expectedSchemaVersion: 1, loadedEntryCount: 100);

        Assert.That(rejection, Is.Not.Null);
        Assert.That(rejection, Does.Contain("no metadata file"));
    }

    [Test]
    public void Validate_rejects_wrong_schema_version()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            MmapCacheMetadata.Save(tempFile, 100, schemaVersion: 1);

            var rejection = MmapCacheMetadata.Validate(tempFile, expectedSchemaVersion: 2, loadedEntryCount: 100);

            Assert.That(rejection, Is.Not.Null);
            Assert.That(rejection, Does.Contain("schema version mismatch"));
        }
        finally
        {
            File.Delete(tempFile);
            MmapCacheMetadata.Delete(tempFile);
        }
    }

    [Test]
    public void Validate_rejects_entry_count_mismatch()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            MmapCacheMetadata.Save(tempFile, 100, schemaVersion: 1);

            var rejection = MmapCacheMetadata.Validate(tempFile, expectedSchemaVersion: 1, loadedEntryCount: 50);

            Assert.That(rejection, Is.Not.Null);
            Assert.That(rejection, Does.Contain("entry count mismatch"));
        }
        finally
        {
            File.Delete(tempFile);
            MmapCacheMetadata.Delete(tempFile);
        }
    }

    [Test]
    public void Validate_rejects_stale_cache_older_than_48h()
    {
        var tempFile = Path.GetTempFileName();
        var metaPath = MmapCacheMetadata.GetMetadataPath(tempFile);
        try
        {
            // Write metadata with an old SavedAtUtc
            MmapCacheMetadata.Save(tempFile, 100, schemaVersion: 1);

            var metadata = MmapCacheMetadata.Load(tempFile);
            Assert.That(metadata, Is.Not.Null);

            // Overwrite with old timestamp
            var oldMetadata = new MmapCacheMetadata
            {
                EntryCount = 100,
                SavedAtUtc = DateTime.UtcNow.AddHours(-49),
                SchemaVersion = 1
            };
            File.WriteAllText(metaPath, System.Text.Json.JsonSerializer.Serialize(oldMetadata));

            var rejection = MmapCacheMetadata.Validate(tempFile, expectedSchemaVersion: 1, loadedEntryCount: 100);

            Assert.That(rejection, Is.Not.Null);
            Assert.That(rejection, Does.Contain("cache too old"));
        }
        finally
        {
            File.Delete(tempFile);
            File.Delete(metaPath);
        }
    }

    [Test]
    public void Delete_removes_metadata_file()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            MmapCacheMetadata.Save(tempFile, 10, 1);
            var metaPath = MmapCacheMetadata.GetMetadataPath(tempFile);

            Assert.That(File.Exists(metaPath), Is.True);

            MmapCacheMetadata.Delete(tempFile);

            Assert.That(File.Exists(metaPath), Is.False);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    [Test]
    public async Task PageViewMmapCache_save_creates_metadata_and_load_validates()
    {
        var pageViewCache = R<PageViewMmapCache>();
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();

        var views = new List<PageViewSummaryWithId>
        {
            new(5, DateTime.UtcNow.Date, 1, DateTime.UtcNow),
            new(3, DateTime.UtcNow.Date.AddDays(-1), 1, DateTime.UtcNow.AddDays(-1))
        };

        // Save and verify metadata is created
        pageViewCache.SaveAllPageViews(views);
        var metadata = MmapCacheMetadata.Load(pageViewCache.FilePath);

        Assert.That(metadata, Is.Not.Null);
        Assert.That(metadata!.EntryCount, Is.EqualTo(2));
        Assert.That(metadata.SchemaVersion, Is.EqualTo(PageViewMmapCache.SchemaVersion));

        // Load should succeed with valid metadata
        var loaded = pageViewCache.LoadPageViews();
        Assert.That(loaded.Count, Is.EqualTo(2));

        await Task.CompletedTask;
    }

    [Test]
    public async Task PageViewMmapCache_load_rejects_cache_without_metadata()
    {
        var pageViewCache = R<PageViewMmapCache>();
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();

        var views = new List<PageViewSummaryWithId>
        {
            new(5, DateTime.UtcNow.Date, 1, DateTime.UtcNow)
        };

        // Save normally (creates both .mmap and .meta)
        pageViewCache.SaveAllPageViews(views);

        // Delete only the metadata file to simulate legacy or corrupt state
        MmapCacheMetadata.Delete(pageViewCache.FilePath);

        // Load should reject and return empty
        var loaded = pageViewCache.LoadPageViews();
        Assert.That(loaded, Is.Empty);

        await Task.CompletedTask;
    }

    [Test]
    public async Task PageViewMmapCache_load_rejects_tampered_entry_count()
    {
        var pageViewCache = R<PageViewMmapCache>();
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();

        var views = new List<PageViewSummaryWithId>
        {
            new(5, DateTime.UtcNow.Date, 1, DateTime.UtcNow),
            new(3, DateTime.UtcNow.Date.AddDays(-1), 1, DateTime.UtcNow.AddDays(-1))
        };

        // Save normally
        pageViewCache.SaveAllPageViews(views);

        // Tamper with metadata to have wrong count
        var metaPath = MmapCacheMetadata.GetMetadataPath(pageViewCache.FilePath);
        var tamperedMetadata = new MmapCacheMetadata
        {
            EntryCount = 999,
            SavedAtUtc = DateTime.UtcNow,
            SchemaVersion = PageViewMmapCache.SchemaVersion
        };
        File.WriteAllText(metaPath, System.Text.Json.JsonSerializer.Serialize(tamperedMetadata));

        // Load should reject due to entry count mismatch
        var loaded = pageViewCache.LoadPageViews();
        Assert.That(loaded, Is.Empty);

        await Task.CompletedTask;
    }

    [Test]
    public async Task PageChangeMmapCache_save_creates_metadata_and_load_validates()
    {
        var pageChangeCache = R<PageChangeMmapCache>();
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();

        var changes = new List<PageChangeSummary>
        {
            new(1, 10, 1, "{}", 1, 0, DateTime.UtcNow)
        };

        pageChangeCache.SaveAllPageChanges(changes);

        var metadata = MmapCacheMetadata.Load(pageChangeCache.FilePath);
        Assert.That(metadata, Is.Not.Null);
        Assert.That(metadata!.EntryCount, Is.EqualTo(1));

        var loaded = pageChangeCache.LoadPageChanges();
        Assert.That(loaded.Count, Is.EqualTo(1));

        await Task.CompletedTask;
    }

    [Test]
    public async Task QuestionViewMmapCache_save_creates_metadata_and_load_validates()
    {
        var questionViewCache = R<QuestionViewMmapCache>();
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();

        var views = new List<QuestionViewSummaryWithId>
        {
            new(7, DateTime.UtcNow.Date, 1, DateTime.UtcNow)
        };

        questionViewCache.SaveAllQuestionViews(views);

        var metadata = MmapCacheMetadata.Load(questionViewCache.FilePath);
        Assert.That(metadata, Is.Not.Null);
        Assert.That(metadata!.EntryCount, Is.EqualTo(1));

        var loaded = questionViewCache.LoadQuestionViews();
        Assert.That(loaded.Count, Is.EqualTo(1));

        await Task.CompletedTask;
    }

    [Test]
    public async Task DeleteAllCacheFiles_removes_data_and_metadata()
    {
        var pageViewCache = R<PageViewMmapCache>();
        var pageChangeCache = R<PageChangeMmapCache>();
        var questionViewCache = R<QuestionViewMmapCache>();
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();

        // Save data to all caches
        pageViewCache.SaveAllPageViews(new List<PageViewSummaryWithId>
        {
            new(1, DateTime.UtcNow.Date, 1, DateTime.UtcNow)
        });
        pageChangeCache.SaveAllPageChanges(new List<PageChangeSummary>
        {
            new(1, 10, 1, "{}", 1, 0, DateTime.UtcNow)
        });
        questionViewCache.SaveAllQuestionViews(new List<QuestionViewSummaryWithId>
        {
            new(1, DateTime.UtcNow.Date, 1, DateTime.UtcNow)
        });

        // Verify metadata files exist
        Assert.That(File.Exists(MmapCacheMetadata.GetMetadataPath(pageViewCache.FilePath)), Is.True);
        Assert.That(File.Exists(MmapCacheMetadata.GetMetadataPath(pageChangeCache.FilePath)), Is.True);
        Assert.That(File.Exists(MmapCacheMetadata.GetMetadataPath(questionViewCache.FilePath)), Is.True);

        // DeleteAll
        mmapCacheRefreshService.DeleteAllCacheFiles();

        // Verify both data and metadata are gone
        Assert.That(File.Exists(pageViewCache.FilePath), Is.False);
        Assert.That(File.Exists(MmapCacheMetadata.GetMetadataPath(pageViewCache.FilePath)), Is.False);
        Assert.That(File.Exists(pageChangeCache.FilePath), Is.False);
        Assert.That(File.Exists(MmapCacheMetadata.GetMetadataPath(pageChangeCache.FilePath)), Is.False);
        Assert.That(File.Exists(questionViewCache.FilePath), Is.False);
        Assert.That(File.Exists(MmapCacheMetadata.GetMetadataPath(questionViewCache.FilePath)), Is.False);

        await Task.CompletedTask;
    }

    [Test]
    public async Task EntityCacheInit_falls_back_to_db_when_cache_has_no_metadata()
    {
        // This simulates a legacy cache file (pre-metadata) or test-contaminated file
        await ClearData();

        var context = NewPageContext();
        var page = context.AddAndGet("Test Page Metadata Fallback");
        context.Persist();

        var yesterday = DateTime.UtcNow.Date.AddDays(-1);
        var twoDaysAgo = DateTime.UtcNow.Date.AddDays(-2);
        AddPageViewsToDatabase(page.Id, new[]
        {
            new TestViews(yesterday, 4),
            new TestViews(twoDaysAgo, 3)
        });

        var pageViewCache = R<PageViewMmapCache>();
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();

        // Write a raw mmap file WITHOUT metadata (simulating legacy/test state)
        var rawViews = new List<PageViewSummaryWithId>
        {
            new(1, DateTime.UtcNow.Date, page.Id, DateTime.UtcNow)
        };
        var bytes = MessagePack.MessagePackSerializer.Serialize(rawViews);
        File.WriteAllBytes(pageViewCache.FilePath, bytes);
        // No metadata file written!

        // EntityCache init should reject the cache-without-metadata and fall back to DB
        SimulateEntityCacheFirstStart("_Test_NoMetadataFallback");

        var pageCacheItem = EntityCache.GetPage(page.Id);

        // Should have views from DB (4+3=7), not from the raw file (1)
        Assert.That(pageCacheItem?.TotalViews, Is.EqualTo(7));

        // After fallback, the cache should now have valid metadata
        // GetAllEager returns aggregated summaries: 2 entries (yesterday + twoDaysAgo)
        var metadata = MmapCacheMetadata.Load(pageViewCache.FilePath);
        Assert.That(metadata, Is.Not.Null);
        Assert.That(metadata!.EntryCount, Is.GreaterThanOrEqualTo(2));
    }

    [Test]
    public async Task AppendPageView_updates_metadata()
    {
        var pageViewCache = R<PageViewMmapCache>();
        var mmapCacheRefreshService = R<MmapCacheRefreshService>();
        mmapCacheRefreshService.DeleteAllCacheFiles();

        // Save initial views
        var views = new List<PageViewSummaryWithId>
        {
            new(5, DateTime.UtcNow.Date, 1, DateTime.UtcNow)
        };
        pageViewCache.SaveAllPageViews(views);

        var metadataBefore = MmapCacheMetadata.Load(pageViewCache.FilePath);
        Assert.That(metadataBefore!.EntryCount, Is.EqualTo(1));

        // Append one more view
        pageViewCache.AppendPageView(new PageViewSummaryWithId(3, DateTime.UtcNow.Date.AddDays(-1), 2, DateTime.UtcNow));

        var metadataAfter = MmapCacheMetadata.Load(pageViewCache.FilePath);
        Assert.That(metadataAfter!.EntryCount, Is.EqualTo(2));

        // Load should validate successfully
        var loaded = pageViewCache.LoadPageViews();
        Assert.That(loaded.Count, Is.EqualTo(2));

        await Task.CompletedTask;
    }

    private void AddPageViewsToDatabase(int pageId, IEnumerable<TestViews> viewData)
    {
        var pageViewRepo = R<PageViewRepo>();

        foreach (var data in viewData)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var sql = @"
                    INSERT INTO pageview (Page_id, User_id, UserAgent, DateCreated, DateOnly) 
                    VALUES (:pageId, :userId, :userAgent, :dateCreated, :dateOnly)";

                pageViewRepo.Session.CreateSQLQuery(sql)
                    .SetParameter("pageId", pageId)
                    .SetParameter("userId", 1)
                    .SetParameter("userAgent", "metadata test")
                    .SetParameter("dateCreated", data.DateTimeNow)
                    .SetParameter("dateOnly", data.DateTimeNow.Date)
                    .ExecuteUpdate();
            }
        }

        pageViewRepo.Flush();
    }

    private void SimulateEntityCacheFirstStart(string initMsg = "")
    {
        EntityCache.Clear();
        EntityCache.IsFirstStart = true;
        var entityCacheInitializer = R<EntityCacheInitializer>();
        entityCacheInitializer.Init(initMsg);
    }
}
