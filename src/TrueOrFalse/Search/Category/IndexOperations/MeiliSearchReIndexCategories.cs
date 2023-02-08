﻿using AutofacContrib.SolrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Meilisearch;

namespace TrueOrFalse.Search
{
    public class MeiliSearchReIndexCategories : IRegisterAsInstancePerLifetime

    {
        public MeilisearchClient _client { get; }
        public MeiliSearchReIndexCategories()
        {
            _client = new MeilisearchClient(MeiliSearchKonstanten.Url, MeiliSearchKonstanten.MasterKey);
        }

        public async Task Go()
        {
            await _client.DeleteIndexAsync(MeiliSearchKonstanten.Categories);
            var allCateogoriesFromDb = Sl.CategoryRepo.GetAll();

            var meiliSearchCategories = allCateogoriesFromDb.Select(c => new MeiliSearchCategoryMap
            {
                Id = c.Id,
                Name = c.Name,
                CreatorId =  c.Creator == null ? -1 : c.Creator.Id,
                DateCreated = c.DateCreated,
                Description = c.Description,
                QuestionCount = c.CountQuestions
            });

            var index = _client.Index(MeiliSearchKonstanten.Categories);
            await index.AddDocumentsAsync(meiliSearchCategories);
        }
    }
}