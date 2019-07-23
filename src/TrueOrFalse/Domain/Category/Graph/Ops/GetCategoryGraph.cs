using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Helpers;
using System.Web.Mvc;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel.Output.Sorting;
using GraphJsonDtos;
using Newtonsoft.Json.Linq;

public class GetCategoryGraph
{

    public static JsonResult AsJson(Category category)
    {
        var graphData = Get(category);

        var links = GetLinks(graphData);
        var nodes = GetNodes(category, graphData);

        AssignNodeLevels(nodes, links);

        return new JsonResult
        {
            Data = new
            {
                nodes = nodes,
                links = links
            }
        };
    }

    private static List<Node> GetNodes(Category category, CategoryGraph graphData)
    {
        var nodes = graphData.nodes.Select((node, index) =>
            new Node
            {
                Id = index,
                CategoryId = node.Category.Id,
                Title = (node.Category.Name).Replace("\"", ""),
                Knowledge = KnowledgeSummaryLoader.RunFromMemoryCache(category, Sl.SessionUser.UserId),
            }).ToList();
        return nodes;
    }

    private static List<Link> GetLinks(CategoryGraph graphData)
    {
        var links = new List<Link>();
        foreach (var link in graphData.links)
        {
            var parentIndex = graphData.nodes.FindIndex(node => node.Category == link.Parent);
            var childIndex = graphData.nodes.FindIndex(node => node.Category == link.Child);
            if (childIndex >= 0 || parentIndex >= 0)
                links.Add(new Link
                {
                    source = parentIndex,
                    target = childIndex,
                });
        }

        return links;
    }

    private static void AssignNodeLevels(IEnumerable<Node> nodes, List<Link> links)
    {
        var currentLvl = 0;
        var maxLevels = 7;
        var allNodes = nodes;
        var allLinks = links;
        var rootNode = nodes.First(Node => Node.Id == 0);
        var rootId = rootNode.CategoryId;
        var inputId = new List<int>();

        inputId.Add(0);

        for (currentLvl = 0; currentLvl < maxLevels; currentLvl++)
        {
            foreach (var node in allNodes)
            {
                if (node.CategoryId != rootId)
                {
                    foreach (var link in allLinks)
                    {
                        if (node.Level == 0 && inputId.Contains(link.source) && link.target == node.Id)
                        {
                            node.Level = currentLvl;
                            if (!inputId.Contains(node.Id))
                                inputId.Add(node.Id);
                        }
                    }
                }
                else
                {
                    node.Level = currentLvl;
                }
            }
        }
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