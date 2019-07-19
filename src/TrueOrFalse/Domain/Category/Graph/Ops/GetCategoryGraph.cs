using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GraphJsonDtos;
public class GetCategoryGraph
{
    public static JsonResult AsJson(Category category)
    {
        var graphData = Get(category);

        var links = new List<Link>();
        foreach (var link in graphData.links)
        {
            var parentIndex = graphData.nodes.FindIndex(node => node.Category == link.Parent);
            var childIndex = graphData.nodes.FindIndex(node => node.Category == link.Child);
            if (childIndex >= 0 || parentIndex >= 0)
                links.Add(new Link { source = parentIndex, target = childIndex });
        }

        var nodes = graphData.nodes.Select(node => 
            new Node
            {
                CategoryId = node.Category.Id,
                Text = (node.Category.Name).Replace("\"", "")
            });

        return new JsonResult
        {
            Data = new
            {
                nodes = nodes,
                links = links
            }
        };
    }

    public static CategoryGraph Get(Category category)
    {
        var descendants = GetCategoriesDescendants.WithAppliedRules(category);

        var nodes = new List<CategoryNode>{new CategoryNode{Category = category}};

        foreach (var descendant in descendants)
            nodes.Add(new CategoryNode {Category = descendant});

        var links = new List<CategoryLink>();
        foreach (var categoryNode in nodes)
        {
            var categoryNodeLinks = GetLinksFromCategory(
                categoryNode.Category,
                Sl.CategoryRepo.GetChildren(categoryNode.Category.Id).ToList()
            );

            links.AddRange(categoryNodeLinks);
        }
        
        return new CategoryGraph
        {
            nodes = nodes,
            links = links
        };
    }

    private static IEnumerable<CategoryLink> GetLinksFromCategory(Category parent, List<Category> children) => 
        children.Select(child => new CategoryLink{
            Parent = parent,
            Child = child
        });
}