﻿using Microsoft.AspNetCore.Http;

public class ContextUtil
{
    private readonly HttpContext? _httpContext;

    public ContextUtil(HttpContext? httpContext)
    {
        _httpContext = httpContext;
    }

    public bool UseWebConfig => Settings.UseWebConfig;

    public bool IsWebContext => _httpContext != null;

    public string GetFilePath(string fileName)
    {
        return Path.Combine(AppContext.BaseDirectory, $@"~/{fileName}");
    }
}