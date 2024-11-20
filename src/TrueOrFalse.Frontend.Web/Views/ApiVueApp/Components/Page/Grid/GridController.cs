﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static PageGridManager;

namespace VueApp;

public class GridController(
    PermissionCheck _permissionCheck,
    SessionUser _sessionUser,
    ImageMetaDataReadingRepo _imageMetaDataReadingRepo,
    IHttpContextAccessor _httpContextAccessor,
    KnowledgeSummaryLoader _knowledgeSummaryLoader,
    QuestionReadingRepo _questionReadingRepo) : Controller
{
    [HttpGet]
    public GetItemJson GetItem([FromRoute] int id)
    {
        var topic = EntityCache.GetPage(id);
        if (topic == null)
            return new GetItemJson(false, FrontendMessageKeys.Error.Default);
        if (!_permissionCheck.CanView(topic))
            return new GetItemJson(false, FrontendMessageKeys.Error.Page.MissingRights);

        var gridItem = new PageGridManager(
                _permissionCheck,
                _sessionUser,
                _imageMetaDataReadingRepo,
                _httpContextAccessor,
                _knowledgeSummaryLoader,
                _questionReadingRepo)
            .BuildGridPageItem(topic);

        return new GetItemJson(true, "", gridItem);
    }

    public readonly record struct GetItemJson(
        bool Success,
        string MessageKey = "",
        GridPageItem? Data = null);
}