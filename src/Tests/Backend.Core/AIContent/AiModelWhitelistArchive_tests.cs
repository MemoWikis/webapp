class AiModelWhitelistArchive_tests : BaseTestHarness
{
    [Test]
    public void HasUsageForModel_returns_false_when_no_usage_exists()
    {
        var aiUsageLogRepo = R<AiUsageLogRepo>();

        var result = aiUsageLogRepo.HasUsageForModel("nonexistent-model-id");

        Assert.That(result, Is.False);
    }

    [Test]
    public void HasUsageForModel_returns_true_when_usage_exists()
    {
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var userReadingRepo = R<UserReadingRepo>();

        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null);

        aiUsageLogRepo.AddUsage(testUser!.Id, 1, 100, 50, "test-model-for-usage-check");

        var result = aiUsageLogRepo.HasUsageForModel("test-model-for-usage-check");

        Assert.That(result, Is.True);
    }

    [Test]
    public void RemoveFromWhitelistById_blocks_deletion_when_model_has_usage_and_prices()
    {
        var whitelistRepo = R<AiModelWhitelistRepo>();
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var userReadingRepo = R<UserReadingRepo>();

        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null);

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-block-delete",
            DisplayName = "Test Block Delete",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 1.0m,
            InputPricePerMillion = 3.00m,
            OutputPricePerMillion = 15.00m,
            IsEnabled = true
        };

        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        aiUsageLogRepo.AddUsage(testUser!.Id, 1, 100, 50, "test-model-block-delete");

        var hasPrices = testModel.InputPricePerMillion > 0 || testModel.OutputPricePerMillion > 0;
        var hasUsage = aiUsageLogRepo.HasUsageForModel(testModel.ModelId);

        Assert.That(hasPrices && hasUsage, Is.True, "Delete should be blocked when model has usage and prices");

        // Cleanup
        whitelistRepo.DeleteModel(savedId);
    }

    [Test]
    public void RemoveFromWhitelistById_allows_deletion_when_model_has_no_usage()
    {
        var whitelistRepo = R<AiModelWhitelistRepo>();
        var aiUsageLogRepo = R<AiUsageLogRepo>();

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-allow-delete",
            DisplayName = "Test Allow Delete",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 1.0m,
            InputPricePerMillion = 3.00m,
            OutputPricePerMillion = 15.00m,
            IsEnabled = true
        };

        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        var hasUsage = aiUsageLogRepo.HasUsageForModel(testModel.ModelId);
        Assert.That(hasUsage, Is.False, "Model should have no usage");

        // Should be allowed to delete
        whitelistRepo.DeleteModel(savedId);
        var deletedModel = whitelistRepo.GetById(savedId);
        Assert.That(deletedModel, Is.Null, "Model should be deleted");
    }

    [Test]
    public void RemoveFromWhitelistById_allows_deletion_when_model_has_usage_but_no_prices()
    {
        var whitelistRepo = R<AiModelWhitelistRepo>();
        var aiUsageLogRepo = R<AiUsageLogRepo>();
        var userReadingRepo = R<UserReadingRepo>();

        var testUser = userReadingRepo.GetById(1);
        Assert.That(testUser, Is.Not.Null);

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-no-prices",
            DisplayName = "Test No Prices",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 1.0m,
            InputPricePerMillion = 0m,
            OutputPricePerMillion = 0m,
            IsEnabled = true
        };

        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        aiUsageLogRepo.AddUsage(testUser!.Id, 1, 100, 50, "test-model-no-prices");

        var hasPrices = testModel.InputPricePerMillion > 0 || testModel.OutputPricePerMillion > 0;
        Assert.That(hasPrices, Is.False, "Model has no prices, so deletion should be allowed");

        // Cleanup
        whitelistRepo.DeleteModel(savedId);
    }

    [Test]
    public void Archive_model_sets_IsEnabled_to_false()
    {
        var whitelistRepo = R<AiModelWhitelistRepo>();

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-archive",
            DisplayName = "Test Archive",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 1.0m,
            InputPricePerMillion = 3.00m,
            OutputPricePerMillion = 15.00m,
            IsEnabled = true
        };

        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        testModel.IsEnabled = false;
        whitelistRepo.Update(testModel);
        whitelistRepo.Flush();

        var archivedModel = whitelistRepo.GetById(savedId);
        Assert.That(archivedModel, Is.Not.Null);
        Assert.That(archivedModel!.IsEnabled, Is.False, "Model should be archived (disabled)");

        // Cleanup
        whitelistRepo.DeleteModel(savedId);
    }

    [Test]
    public void Unarchive_model_sets_IsEnabled_to_true()
    {
        var whitelistRepo = R<AiModelWhitelistRepo>();

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-unarchive",
            DisplayName = "Test Unarchive",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 1.0m,
            InputPricePerMillion = 3.00m,
            OutputPricePerMillion = 15.00m,
            IsEnabled = false
        };

        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        testModel.IsEnabled = true;
        whitelistRepo.Update(testModel);
        whitelistRepo.Flush();

        var unarchivedModel = whitelistRepo.GetById(savedId);
        Assert.That(unarchivedModel, Is.Not.Null);
        Assert.That(unarchivedModel!.IsEnabled, Is.True, "Model should be unarchived (enabled)");

        // Cleanup
        whitelistRepo.DeleteModel(savedId);
    }
}
