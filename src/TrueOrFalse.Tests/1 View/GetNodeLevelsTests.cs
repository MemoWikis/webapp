using System.Collections.Generic;
using GraphJsonDtos;
using NUnit.Framework;


public class GetLevelsTests
{
    [Test]
    public void GetNodeLevel()
    {
        var nodes = new List<Node>
        {
            new Node{Id = 0, Title = "RootNode"},
            new Node{Id = 1},
            new Node{Id = 2},
            new Node{Id = 3},
            new Node{Id = 4},
            new Node{Id = 5},
        };

        var links = new List<Link>
        {
            new Link(0, 1), //Level 0 -> Level 1
            new Link(0, 2), //Level 0 -> Level 1
            new Link(1, 3), //Level 1 -> Level 2
            new Link(2, 1), //Level 1 -> Level 1
            new Link(3, 4), //Level 2 -> Level 3
            new Link(4, 5), //Level 2 -> Level 3
            new Link(0, 5), //Level 2 -> Level 3
        };

        GetCategoryGraph.Test_AssignNodeLevels(nodes, links);

        Assert.That(nodes[0].Level, Is.EqualTo(0));
        Assert.That(nodes[1].Level, Is.EqualTo(1));
        Assert.That(nodes[4].Level, Is.EqualTo(3));
        Assert.That(nodes[2].Level, Is.EqualTo(1));
        Assert.That(nodes[5].Level, Is.EqualTo(1));
    }

    [Test]
    public void GetLinkLevel()
    {
        var nodes = new List<Node>
        {
            new Node{Id = 0, Level = 0, Title = "RootNode"},
            new Node{Id = 1, Level = 1},
            new Node{Id = 2, Level = 1},
            new Node{Id = 3, Level = 2},
            new Node{Id = 4, Level = 3},
            new Node{Id = 5, Level = 1},
            new Node{Id = 6, Level = -1},
            new Node{Id = 7, Level = -1},
        };

        var links = new List<Link>
        {
            new Link(0, 1), //Level 0 -> Level 1
            new Link(0, 2), //Level 0 -> Level 1
            new Link(1, 3), //Level 1 -> Level 2
            new Link(2, 1), //Level 1 -> Level 1
            new Link(3, 4), //Level 2 -> Level 3
            new Link(4, 5), //Level 2 -> Level 3
            new Link(0, 5), //Level 2 -> Level 3
            new Link(6, 7), //Level 2 -> Level 3
        };

        GetCategoryGraph.Test_AssignLinkLevels(nodes, links);

        Assert.That(links[0].level, Is.EqualTo(1));
        Assert.That(links[1].level, Is.EqualTo(1));
        Assert.That(links[4].level, Is.EqualTo(3));
        Assert.That(links[2].level, Is.EqualTo(2));
        Assert.That(links[7].level, Is.EqualTo(-1));
    }
}