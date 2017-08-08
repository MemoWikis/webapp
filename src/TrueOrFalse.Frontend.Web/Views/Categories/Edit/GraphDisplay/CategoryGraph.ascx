<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryGraphModel>" %>
<button id="CategoryGraphFullscreenButton" class="btn"><i class="fa fa-arrows-alt"></i></button>
<div id="category-graph"></div>
<script type="text/javascript">
    function buildCategoryGraph(targetSelector, width, height) {
        var svg = d3.select(targetSelector).append("svg")
            .attr("width", width)
            .attr("height", height);

        var force = d3.layout.force()
            .distance(200)
            .linkStrength(0.1)
            .charge(-1000)
            .size([width, height]);

        var json = $.parseJSON('<%= @Model.GraphDataString %>');

        force
            .nodes(json.Nodes)
            .links(json.Links)
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

        force.on("tick",
            function() {
                link.attr("x1", d => d.source.x)
                    .attr("y1", d => d.source.y)
                    .attr("x2", d => d.target.x)
                    .attr("y2", d => d.target.y);

                node.attr("transform", d => "translate(" + d.x + "," + d.y + ")");
            });

        $(targetSelector + " text").first().css({ "font-weight": "bold", "fill": "red" });
    }

    function requestFullScreen(element) {
        // Supports most browsers and their versions.
        var requestMethod = element.requestFullScreen || element.webkitRequestFullScreen || element.mozRequestFullScreen || element.msRequestFullScreen;

        if (requestMethod) { // Native full screen.
            requestMethod.call(element);
        } else if (typeof window.ActiveXObject !== "undefined") { // Older IE.
            var wscript = new ActiveXObject("WScript.Shell");
            if (wscript !== null) {
                wscript.SendKeys("{F11}");
            }
        }
    }

    $("#CategoryGraphFullscreenButton").click(function (e) {
        $("body").append($('<div id="CategoryGraphFullscreen">'));
        var fullscreenElement = document.getElementById("CategoryGraphFullscreen");
        requestFullScreen(fullscreenElement);
        var width = $(window).width();
        var height = $(window).height();
        buildCategoryGraph("#CategoryGraphFullscreen", width, height);
    });

    $(document).on('webkitfullscreenchange mozfullscreenchange fullscreenchange', function (e) {
        if (document.webkitIsFullScreen || document.mozFullScreen || document.msFullscreenElement !== null) {
            var fullscreenElement = document.fullscreenElement || document.mozFullScreenElement || document.webkitFullscreenElement;
            if(fullscreenElement == null)
                $("#CategoryGraphFullscreen").remove();
        }
    });

    var width = $("#EditAggregationModal .modal-body").width();
    var height = 600;
    buildCategoryGraph("#category-graph", width , height);
</script>