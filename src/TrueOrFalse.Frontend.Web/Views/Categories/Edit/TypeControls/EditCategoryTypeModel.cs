﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TrueOrFalse;

public class EditCategoryTypeModel : BaseModel
{
    public bool IsEditing;

    public string Name { get; set; }

    public string Description { get; set; }
    public string WikipediaUrl { get; set; }

    public object Model;

    public const string WikipediaInfo = "Falls es einen Wikipedia-Artikel zur Kategorie gibt, gib bitte hier den Link an (z.B. Kategorie 'Lerntheorie' - http://de.wikipedia.org/wiki/Lerntheorie).";
    public const string DescriptionInfo = "Kurze Beschreibung der Kategorie und/oder alternative Bezeichnungen.";

    public const string IsbnInfo = " Bitte mit Bindestrichen angeben. Falls zwei ISBN-Nummern vorhanden sind, verwende bitte immer die längere (13-stellig). Die ISBN ist eine Identifizierungsnummer, die meist auf der Buchrückseite oder im Impressum eines Buchs zu finden ist. ";
     
    public const string IssnInfo = "Die ISSN ist eine Identifizierungsnummer für Zeitungen und Zeitschriften (ähnlich der ISBN für Bücher). Du kannst sie z.B. im Impressum, in Online-Katalogen von Bibliotheken oder auch im Wikipedia-Artikel zu einer Zeitung oder Zeitschrift finden.";


    public EditCategoryTypeModel(Category category, CategoryType type)
    {
        IsEditing = category != null;

        if (category == null)
        {
            var typeModel = HttpContext.Current.Session["RecentCategoryTypeModel"];
            if (typeModel != null)
            {
                if (((ICategoryBase) typeModel).Type == type){
                    Model = typeModel;
                    PopulateFromCategory((Category)HttpContext.Current.Session["RecentCategory"]);
                }
            }

            return;            
        }

        PopulateFromCategory(category);

        if (category.Type == CategoryType.Book)
            Model = CategoryBook.FromJson(category.TypeJson, category);

        if (category.Type == CategoryType.Daily)
            Model = CategoryDaily.FromJson(category.TypeJson, category);
        
        if (category.Type == CategoryType.DailyArticle)
            Model = CategoryDailyArticle.FromJson(category.TypeJson, category);

        if (category.Type == CategoryType.DailyIssue)
            Model = CategoryDailyIssue.FromJson(category.TypeJson, category);

        if (category.Type == CategoryType.Magazine)
            Model = CategoryMagazine.FromJson(category.TypeJson, category);
        
        if (category.Type == CategoryType.MagazineIssue)
            Model = CategoryMagazineIssue.FromJson(category.TypeJson, category);
        
        if (category.Type == CategoryType.VolumeChapter)
            Model = CategoryVolumeChapter.FromJson(category.TypeJson, category);

        if (category.Type == CategoryType.Website)
            Model = CategoryWebsite.FromJson(category.TypeJson, category);
        
        if (category.Type == CategoryType.WebsiteVideo)
            Model = CategoryWebsiteVideo.FromJson(category.TypeJson, category);
    }

    private void PopulateFromCategory(Category category)
    {
        Name = category.Name;
        Description = category.Description;
        WikipediaUrl = category.WikipediaURL;
    }

    public static void SaveToSession(object typeModel, Category category)
    {
        HttpContext.Current.Session["RecentCategoryTypeModel"] = typeModel;
        HttpContext.Current.Session["RecentCategory"] = category;
    }
}
