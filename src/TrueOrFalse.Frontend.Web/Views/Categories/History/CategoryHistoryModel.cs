using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueOrFalse;
using TrueOrFalse.Frontend.Web.Code;

public class CategoryHistoryModel : BaseModel
{
    public int CategoryId { get; set; }
    public string CategoryName;
    public string CategoryUrl;
    public IList<CategoryChangeDayModel> Days;

    public CategoryHistoryModel(Category category, IList<CategoryChange> categoryChanges, int categoryId )
    {
        Data data = new Data(); ;
        if (category == null)
        {
            data = JsonConvert.DeserializeObject<Data>(categoryChanges[categoryChanges.Count - 2].Data);
        }

        CategoryName = category == null ?  data.Name :  category.Name;
        CategoryId = categoryId;
        CategoryUrl = Links.CategoryDetail(CategoryName, CategoryId);

        Days = categoryChanges
            .GroupBy(change => change.DateCreated.Date)
            .OrderByDescending(group => group.Key)
            .Select(group => new CategoryChangeDayModel(
                group.Key, 
                (IList<CategoryChange>) group.OrderByDescending(g => g.DateCreated).ToList()
            ))
            .ToList();
    }
}

public class CategoryChangeDayModel
{
    public string Date;
    public IList<CategoryChangeDetailModel> Items;
    

    public CategoryChangeDayModel(DateTime date, IList<CategoryChange> changes)
    {
        Date = date.ToString("dd.MM.yyyy");
        var categoryId = -1; 
        Items = changes.Select(cc =>
        {
            var data = JsonConvert.DeserializeObject<Data>(cc.Data);
            var typ = "";

            switch (cc.Type)
            {
                case CategoryChangeType.Create: 
                    typ = "Erstellt";
                    break;
                case CategoryChangeType.Update: 
                    typ = "Update";
                    break;
                case CategoryChangeType.Delete: 
                    typ = "Gelöscht";
                    break;
                default: 
                    Logg.r().Error("CategoryHistoryModel CategoryChangeType is invalid");
                    break;
            }

            if (cc.Category == null)
            {
               categoryId =  Sl.Resolve<ISession>().CreateSQLQuery("Select Category_id FROM categorychange where id = " + changes.First().Id).UniqueResult<int>();
              
            }

            return new CategoryChangeDetailModel
            {
                Author = cc.Author,
                AuthorName = cc.Author.Name,
                AuthorImageUrl = new UserImageSettings(cc.Author.Id).GetUrl_85px_square(cc.Author).Url,
                ElapsedTime = TimeElapsedAsText.Run(cc.DateCreated),
                DateTime = cc.DateCreated.ToString("dd.MM.yyyy HH:mm"),
                Time = cc.DateCreated.ToString("HH:mm"),
                CategoryChangeId = cc.Id,
                CategoryId = cc.Category==null ? categoryId : cc.Category.Id,
                CategoryName = data.Name,
                Typ = typ
            };
        }).ToList();
    }
}

public class CategoryChangeDetailModel
{
    public User Author ;
    public string AuthorName;
    public string AuthorImageUrl;
    public string ElapsedTime;
    public string DateTime;
    public string Time;
    public int CategoryChangeId;
    public int CategoryId;
    public string CategoryName;
    public string Typ; 
}

public class Data
{
    public string Name;
    public string Description; 
}