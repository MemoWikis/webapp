<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryGraphModel>" %>

<div id="category-graph"></div>
<script type="text/javascript">
    var width = $("#EditAggregationModal .modal-body").width(),
        height = 600;

    var svg = d3.select("#category-graph").append("svg")
        .attr("width", width)
        .attr("height", height);

    var force = d3.layout.force()
        .gravity(0.05)
        .distance(100)
        .charge(-100)
        .size([width, height]);

    var json = $.parseJSON('<%= Model.GraphDataString %>');

    console.log(json);

            force
                .nodes(json.Nodes)
                .links(json.Links)
                .linkDistance(250)
                .start();

            var link = svg.selectAll(".link")
                .data(json.Links)
                .enter().append("line")
                .attr("class", "link");

            var node = svg.selectAll(".node")
                .data(json.Nodes)
                .enter().append("g")
                .attr("class", "node")
                .call(force.drag);

            node.append("circle")
                .attr("r", 10);

            node.append("text")
                .attr("dx", 12)
                .attr("dy", ".35em")
                .text(d => d.Text);

            force.on("tick", function() {
                link.attr("x1", d => d.source.x)
                    .attr("y1", d => d.source.y)
                    .attr("x2", d => d.target.x)
                    .attr("y2", d => d.target.y);

                node.attr("transform", d => "translate(" + d.x + "," + d.y + ")");
            });

    $("#category-graph text").first().css("font-weight", "bold");
</script>