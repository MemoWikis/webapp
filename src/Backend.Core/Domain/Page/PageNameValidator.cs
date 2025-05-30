﻿using System;
using System.Collections.Generic;
public class PageNameValidator
{
    private static readonly HashSet<string> ForbiddenWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "wissenszentrale",
        "kategorien",
        "fragen",
        "widgets",
        "ueber-memowikis",
        "fuer-lehrer",
        "widget-beispiele",
        "widget-angebote-preislisten",
        "hilfe",
        "impressum",
        "imprint",
        "agb",
        "agbs",
        "jobs",
        "gemeinwohloekonomie",
        "team"
    };

    public bool IsForbiddenName(string name) => !IsValidName(name);

    private bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var processedName = name.Trim().ToLower();
        return !ForbiddenWords.Contains(processedName);
    }

}