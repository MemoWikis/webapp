﻿using Meilisearch;

public class MeilisearchReIndexPages(PageRepository _pageRepository) : IRegisterAsInstancePerLifetime
{
    public MeilisearchClient _client { get; } = new(Settings.MeilisearchUrl, Settings.MeilisearchMasterKey);

    public async Task Run()
    {
        await _client.DeleteIndexAsync(MeilisearchIndices.Pages);
        var allPagesFromDb = _pageRepository.GetAll();

        var meiliSearchPageMaps = allPagesFromDb.Select(c => new MeilisearchPageMap
        {
            Id = c.Id,
            Name = c.Name,
            CreatorName = c.Creator == null ? "Unbekannt" : c.Creator.Name,
            DateCreated = c.DateCreated,
            Description = c.Description,
            Content = c.Content,
            Language = c.Language
        });

        var index = _client.Index(MeilisearchIndices.Pages);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });

        await index.AddDocumentsAsync(meiliSearchPageMaps);
    }

    public async Task RunCache()
    {
        await _client.DeleteIndexAsync(MeilisearchIndices.Pages);

        var pages = EntityCache.GetAllPagesList();

        var meiliSearchPageMaps = pages.Select(c => new MeilisearchPageMap
        {
            Id = c.Id,
            Name = c.Name,
            CreatorName = c.Creator == null ? "Unbekannt" : c.Creator.Name,
            DateCreated = c.DateCreated,
            Description = c.Description,
            Content = c.Content,
            Language = c.Language
        });

        var index = _client.Index(MeilisearchIndices.Pages);
        await index.UpdateFilterableAttributesAsync(new[] { "Language" });

        await index.AddDocumentsAsync(meiliSearchPageMaps);
    }
}