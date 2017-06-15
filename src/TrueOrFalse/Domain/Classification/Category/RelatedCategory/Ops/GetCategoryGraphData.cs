using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphDataAsJson;

public class GetCategoryGraphData
{
    public static JsonResult GetAsJson(Category category)
    {
        var graphData = Get(category);

        var links = new List<Link>();
        foreach (var link in graphData.Links)
        {
            links.Add(new Link { Source = link.Parent.Id, Target = link.Child.Id });
        }

        var nodes = new List<Node>();
        foreach (var node in graphData.Nodes)
        {
            nodes.Add(new Node { CategoryId = node.Category.Id, Text = node.Category.Name });
        }

        return new JsonResult
        {
            Data = new
            {
                Nodes = nodes,
                Links = links
            }
        };
    }

    public static CategoryGraphDataResult Get(Category category)
    {
        var descendants = ModifyRelationsForCategory.GetCategoriesDescendantsWithAppliedRules(category);

        var nodes = new List<CategoryNode>
        {
            new CategoryNode { Category = category }
        };
        foreach (var descendant in descendants)
        {
            nodes.Add(new CategoryNode { Category = descendant });
        }

        var links = new List<CategoryLink>();
        foreach (var categoryNode in nodes)
        {
            var categoryNodeLinks = GetLinksFromCategory(categoryNode.Category,
                Sl.CategoryRepo.GetChildren(categoryNode.Category.Id).ToList());
            links.AddRange(categoryNodeLinks);
        }
        
        return new CategoryGraphDataResult
        {
            Nodes = nodes,
            Links = links
        };
    }

    private static List<CategoryLink> GetLinksFromCategory(Category parent, List<Category> children)
    {
        var categoryLinkList = new List<CategoryLink>();
        foreach (var child in children)
        {
            categoryLinkList.Add(new CategoryLink
            {
                Parent = parent,
                Child = child
            });
        }

        return categoryLinkList;
    }
}

public class CategoryGraphDataResult
{
    public List<CategoryNode> Nodes;
    public List<CategoryLink> Links;
}

public class CategoryLink
{
    public Category Parent;
    public Category Child;
}

public class CategoryNode
{
    public Category Category;
}

namespace GraphDataAsJson
{
    public class Node
    {
        public int CategoryId;
        public string Text;
    }

    public class Link
    {
        public int Source;
        public int Target;
    }
}