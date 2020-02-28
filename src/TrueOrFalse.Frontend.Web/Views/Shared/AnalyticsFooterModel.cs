using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
public class AnalyticsFooterModel 
{

   public IList<Category> AllCategoriesParents;
   public int CategoriesDescendantsCount;
   public string ParentList;
   public int CategoryId;
   public Category Category;
   public bool IsQuestionssite; 

   public AnalyticsFooterModel(Category category, bool isQuestionsSite = false)
   {
       Category = category;
       CategoryId = category.Id;
       GetCategoryRelations();
       IsQuestionssite = isQuestionsSite;
   }


   public void GetCategoryRelations()
   {
       var descendants = GetCategoriesDescendants.WithAppliedRules(Category);
       CategoriesDescendantsCount = descendants.Count;
       AllCategoriesParents = Sl.CategoryRepo.GetAllParents(CategoryId);

       if (AllCategoriesParents.Count > 0)
           ParentList = GetCategoryParentList();
   }


   private string GetCategoryParentList()
   {
       string categoryList = "";
       string parentList;
       foreach (var category in AllCategoriesParents.Take(3))
       {
           categoryList = categoryList + category.Name + ", ";
       }
       if (AllCategoriesParents.Count > 3)
       {
           parentList = categoryList + "...";
       }
       else
       {
           categoryList = categoryList.Remove(categoryList.Length - 2);
           parentList = categoryList;
       }

       return "(" + parentList + ")";
   }
}