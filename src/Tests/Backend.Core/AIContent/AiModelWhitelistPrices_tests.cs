/// <summary>
/// Tests for the AI model whitelist price configuration (InputPricePerMillion, OutputPricePerMillion)
/// </summary>
class AiModelWhitelistPrices_tests : BaseTestHarness
{
    [Test]
    public void AiModelWhitelist_InputPricePerMillion_and_OutputPricePerMillion_can_be_set_and_persisted()
    {
        // Arrange
        var whitelistRepo = R<AiModelWhitelistRepo>();

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-prices",
            DisplayName = "Test Model for Prices",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 1.0m,
            InputPricePerMillion = 3.00m,
            OutputPricePerMillion = 15.00m,
            IsEnabled = true
        };

        // Act
        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        // Re-read from database
        var retrievedModel = whitelistRepo.GetById(savedId);

        // Assert
        Assert.That(retrievedModel, Is.Not.Null, "Model should be retrievable");
        Assert.That(retrievedModel!.InputPricePerMillion, Is.EqualTo(3.00m));
        Assert.That(retrievedModel.OutputPricePerMillion, Is.EqualTo(15.00m));

        // Cleanup
        whitelistRepo.DeleteModel(savedId);
    }

    [Test]
    public void AiModelWhitelist_prices_default_to_zero()
    {
        // Arrange
        var whitelistRepo = R<AiModelWhitelistRepo>();

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-default-prices",
            DisplayName = "Test Model Default Prices",
            Provider = AiModelProvider.OpenAI,
            TokenCostMultiplier = 1.0m,
            IsEnabled = true
            // InputPricePerMillion and OutputPricePerMillion not set
        };

        // Act
        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        var retrievedModel = whitelistRepo.GetById(savedId);

        // Assert
        Assert.That(retrievedModel, Is.Not.Null);
        Assert.That(retrievedModel!.InputPricePerMillion, Is.EqualTo(0m));
        Assert.That(retrievedModel.OutputPricePerMillion, Is.EqualTo(0m));

        // Cleanup
        whitelistRepo.DeleteModel(savedId);
    }

    [Test]
    public void AiModelWhitelist_prices_can_be_updated()
    {
        // Arrange
        var whitelistRepo = R<AiModelWhitelistRepo>();

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-update-prices",
            DisplayName = "Test Model Update Prices",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 1.0m,
            InputPricePerMillion = 1.00m,
            OutputPricePerMillion = 5.00m,
            IsEnabled = true
        };

        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        // Act - Update prices
        var modelToUpdate = whitelistRepo.GetById(savedId);
        modelToUpdate!.InputPricePerMillion = 2.50m;
        modelToUpdate.OutputPricePerMillion = 10.00m;
        whitelistRepo.Update(modelToUpdate);
        whitelistRepo.Flush();

        // Re-read from database
        var updatedModel = whitelistRepo.GetById(savedId);

        // Assert
        Assert.That(updatedModel!.InputPricePerMillion, Is.EqualTo(2.50m));
        Assert.That(updatedModel.OutputPricePerMillion, Is.EqualTo(10.00m));

        // Cleanup
        whitelistRepo.DeleteModel(savedId);
    }

    [Test]
    public void AiModelWhitelist_prices_support_fractional_cents()
    {
        // Arrange
        var whitelistRepo = R<AiModelWhitelistRepo>();

        // Claude 3.5 Sonnet actual prices: $3.00 input, $15.00 output per million
        // GPT-4o mini: $0.15 input, $0.60 output per million
        var testModel = new AiModelWhitelist
        {
            ModelId = "test-model-fractional-prices",
            DisplayName = "Test Model Fractional Prices",
            Provider = AiModelProvider.OpenAI,
            TokenCostMultiplier = 0.5m,
            InputPricePerMillion = 0.1500m,  // $0.15 per million
            OutputPricePerMillion = 0.6000m,  // $0.60 per million
            IsEnabled = true
        };

        // Act
        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        var retrievedModel = whitelistRepo.GetById(savedId);

        // Assert
        Assert.That(retrievedModel!.InputPricePerMillion, Is.EqualTo(0.1500m));
        Assert.That(retrievedModel.OutputPricePerMillion, Is.EqualTo(0.6000m));

        // Cleanup
        whitelistRepo.DeleteModel(savedId);
    }

    [Test]
    public async Task UpdateWhitelistPrices_API_updates_model_prices()
    {
        // Arrange
        var whitelistRepo = R<AiModelWhitelistRepo>();

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-api-update-prices",
            DisplayName = "Test API Update Prices",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 1.0m,
            InputPricePerMillion = 0m,
            OutputPricePerMillion = 0m,
            IsEnabled = true
        };

        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        try
        {
            // Act - Call API to update prices
            var request = new UpdatePricesRequest(savedId, 3.00m, 15.00m);

            var result = await _testHarness.ApiPostJson<UpdatePricesRequest, VueMaintenanceResult>(
                "apiVue/VueMaintenance/UpdateWhitelistPrices",
                request);

            // Assert
            Assert.That(result.Success, Is.True);

            var updatedModel = whitelistRepo.GetById(savedId);
            Assert.That(updatedModel!.InputPricePerMillion, Is.EqualTo(3.00m));
            Assert.That(updatedModel.OutputPricePerMillion, Is.EqualTo(15.00m));
        }
        finally
        {
            // Cleanup
            whitelistRepo.DeleteModel(savedId);
        }
    }

    [Test]
    public async Task GetWhitelistedAiModels_API_returns_prices()
    {
        // Arrange
        var whitelistRepo = R<AiModelWhitelistRepo>();

        var testModel = new AiModelWhitelist
        {
            ModelId = "test-api-get-prices",
            DisplayName = "Test API Get Prices",
            Provider = AiModelProvider.Anthropic,
            TokenCostMultiplier = 2.0m,
            InputPricePerMillion = 5.00m,
            OutputPricePerMillion = 25.00m,
            IsEnabled = true
        };

        whitelistRepo.SaveModel(testModel);
        var savedId = testModel.Id;

        try
        {
            // Act - Call API to get whitelisted models
            var result = await _testHarness.ApiGet<GetWhitelistedModelsResponse>(
                "apiVue/VueMaintenance/GetWhitelistedAiModels");

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Models, Is.Not.Empty);

            var model = result.Models.FirstOrDefault(m => m.ModelId == "test-api-get-prices");
            Assert.That(model, Is.Not.Null, "Test model should be in the response");
            Assert.That(model!.InputPricePerMillion, Is.EqualTo(5.00m));
            Assert.That(model.OutputPricePerMillion, Is.EqualTo(25.00m));
        }
        finally
        {
            // Cleanup
            whitelistRepo.DeleteModel(savedId);
        }
    }

    // Request type for update prices API
    private record UpdatePricesRequest(int Id, decimal InputPricePerMillion, decimal OutputPricePerMillion);

    // Response types for API calls
    private record VueMaintenanceResult(bool Success, string Data);

    private record WhitelistedModel(
        int Id,
        string Provider,
        string ModelId,
        string DisplayName,
        decimal TokenCostMultiplier,
        decimal InputPricePerMillion,
        decimal OutputPricePerMillion);

    private record GetWhitelistedModelsResponse(bool Success, List<WhitelistedModel> Models);
}
