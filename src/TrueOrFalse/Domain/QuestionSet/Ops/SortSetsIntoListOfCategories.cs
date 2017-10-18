using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

public class SortSetsIntoListOfCategories
{

    /// <summary>
    /// Inserts each set to the most appropriate position within a list of categories, assuming the list of categories is ordered from less to more specific
    /// (e.g. via OrderCategoriesByQuestionCountAndLevel.Run)
    /// </summary>
    /// <returns>List of objects, each object either being a Set or a Category</returns>
    public static IList<object> Run(IList<Category> categories, IList<Set> sets)
    {
        var output = new List<object>();
        categories.ForEach(c => output.Add(c));
        foreach (var set in sets)
        {
            var insertAtIndex = -1;
            foreach (var catOrSetInWish in output) //check if set belongs to category, look for last one (= most specific) than insert set after that one
            {
                if (catOrSetInWish is Category && ((Category)catOrSetInWish).AggregatedCategories(true)
                    .Intersect(set.Categories)
                    .Any())
                {
                    insertAtIndex = output.IndexOf(catOrSetInWish) + 1;
                }
            }
            if (insertAtIndex == -1)
            {
                //if no inserting point found (= a "parent" category for the set does not exist), then find suitable place according to questioncount
                insertAtIndex = output.FindLastIndex(
                                    o =>
                                    {
                                        if (o is Category)
                                            return ((Category)o).CountQuestionsAggregated > set.QuestionsPublicCount();
                                        if (o is Set)
                                            return ((Set)o).QuestionsPublicCount() > set.QuestionsPublicCount();
                                        return false;
                                    }) + 1;
                if (insertAtIndex > 0)
                { //If Index > 0 then check if set would interrupt a parent-child-relation of the categories. Then postpone insertation until a category that is not a child of anyone before.
                    var aggregatedAggregateCatsOfPreviousItems = new List<Category>();
                    foreach (var followingCatOrSet in output)
                    { //to check if set would interrupt parent-child relation, consider aggregated categories of all categories in the previous line to see whether following categories are children of any of the previous ones

                        //ignore the categories that would come before the set, but don't use skip in foreach to keep the first elements in the loop and aggregate their aggregatedCategories
                        if (output.IndexOf(followingCatOrSet) >= insertAtIndex)
                        {
                            if (followingCatOrSet is Category)
                            {
                                if (aggregatedAggregateCatsOfPreviousItems.Contains((Category)followingCatOrSet))
                                {
                                    insertAtIndex = output.IndexOf(followingCatOrSet) + 1;
                                }
                                else //at this insertAtIndex, set would not interrupt category tree
                                {
                                    insertAtIndex = output.IndexOf(followingCatOrSet);
                                    break;
                                }
                            }

                        }

                        if (followingCatOrSet is Category)
                            aggregatedAggregateCatsOfPreviousItems.AddRange(((Category)followingCatOrSet)
                                .AggregatedCategories(true));
                    }
                }

            }
            output.Insert(insertAtIndex, set);
        }
        return output;
    }
}
