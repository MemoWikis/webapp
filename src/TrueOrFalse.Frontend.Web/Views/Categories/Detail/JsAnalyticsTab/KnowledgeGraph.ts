declare const graphJsonString: any;

var maxLevel = 3;
var maxNodeCount = 50;
var dataIsReady = false;
var showKnowledgeBar = true;
var currentGraph = "radialNodeGraph";

declare var graphNodes: any;
declare var graphLinks: any;
declare var graphData: any;

declare var nodeCount: any;
declare var graphDepth: any;

class KnowledgeGraph {

    static initGraphData() {
        const _graphData = JSON.parse(graphJsonString);
        const _nodes = _graphData.nodes;
        nodeCount = _nodes.length;
        if (_nodes.some(item => item.Level == -1))
            graphDepth = -1;
        else {
            graphDepth = _nodes.sort(function(a, b) {
                    return parseFloat(b['Level']) - parseFloat(a['Level']);
                })[0]['Level'];
        }
    }

    static updateDropdown(count) {

        $('#graphMaxNodeCount').attr('max', count);
        $('#nodeCountValue').attr('max', count);

        if (maxNodeCount > count) {
            $('#graphMaxNodeCount').val(count.toString());
            $('#nodeCountValue').val(count.toString());
        }
    }

    static setGraphData(level, count, knowledgeBar) {
        graphData = JSON.parse(graphJsonString);
        maxLevel = level;
        maxNodeCount = count;
        showKnowledgeBar = knowledgeBar;
        dataIsReady = false;
        if (currentGraph == "rectangleNodeGraph")
            this.loadRectangleNodeGraph();
        else
            this.loadRadialNodeGraph();
    }

    static limitNodeLevel() {
        if (maxLevel > -1) {
            return graphNodes = graphData.nodes
                .filter(function (d) { return d.Level >= 0; })
                .filter(function (d) { return d.Level <= maxLevel; });
        } else {
            return graphNodes = graphData.nodes;
        }
    }

    static limitLinkLevel() {
        if (maxLevel > -1) {
            return graphLinks = graphData.links
                .filter(function (d) { return d.level >= 0; })
                .filter(function (d) { return d.level <= maxLevel; });
        } else {
            return graphLinks = graphData.links;
        }
    }

    static async limitGraphNodes() {
        graphNodes = await this.limitNodeLevel();
        graphLinks = await this.limitLinkLevel();
        this.updateDropdown(graphNodes.length);
        if (maxNodeCount > -1 && graphNodes.length > maxNodeCount) {
            graphNodes = graphNodes.slice(0, maxNodeCount);
            graphLinks = graphLinks
                .filter(function(l) { return l.source < maxNodeCount; })
                .filter(function(l) { return l.target < maxNodeCount; });
            return dataIsReady = true;
        }
    }

    static async loadRadialNodeGraph() {
        currentGraph = 'radialNodeGraph';

        var width = 810;
        var height = 600;
        var color = d3.scaleOrdinal(d3.schemeCategory10);

        var nodes = [];
        var links = [];

        if (!dataIsReady)
            await this.limitGraphNodes();

        nodes = graphNodes;
        links = graphLinks;

        var label = {
            'nodes': [],
            'links': [],
        };

        nodes.forEach(function(d, i) {
            label.nodes.push({ node: d });
            label.nodes.push({ node: d });
            label.links.push({

                source: i * 2,
                target: i * 2 + 1
            });
        });

        var labelLayout = d3.forceSimulation(label.nodes)
            .force("charge", d3.forceManyBody().strength(-50))
            .force("link", d3.forceLink(label.links).distance(0).strength(1));

        var graphLayout = d3.forceSimulation(nodes)
            .force("charge", d3.forceManyBody().strength(-3000))
            .force("center", d3.forceCenter(width / 2, height / 2))
            .force("x", d3.forceX(width / 2).strength(1))
            .force("y", d3.forceY(height / 2).strength(1))
            .force("link", d3.forceLink(links).distance(200).strength(1))
            .on("tick", ticked);

        var adjlist = [];

        links.forEach(function (d) {
            adjlist[d.source.index + "-" + d.target.index] = true;
            adjlist[d.target.index + "-" + d.source.index] = true;
        });

        function neigh(a, b) {
            return a == b || adjlist[a + "-" + b];
        }

        var svg = d3.select("#graph-body").attr("width", width).attr("height", height);
        svg.append("rect")
            .attr("width", "100%")
            .attr("height", "100%")
            .attr("fill", "white");

        var container = svg.append("g");

        svg.call(
            d3.zoom()
                .scaleExtent([.1, 4])
                .on("zoom", function () { container.attr("transform", d3.event.transform); })
        );

        var link = container.append("g").attr("class", "links")
            .selectAll("line")
            .data(links)
            .enter()
            .append("line")
            .attr("stroke", "#999")
            .attr("stroke-width", "1px");

        var node = container.append("g").attr("class", "nodes")
            .selectAll("g")
            .data(nodes)
            .enter()
            .append("circle")
            .attr("r", function (d) {
                d.weight = link.filter(function (l) {
                    return l.source.index == d.index || l.target.index == d.index;
                }).size();
                const minRadius = 5;
                return minRadius + ((Math.sqrt(d.weight) * 4) - 4 );
            })
            .attr("fill", function (d) { return color(d.group); });

        node.on("mouseover", focus).on("mouseout", unfocus);

        node.call(
            d3.drag()
                .on("start", dragstarted)
                .on("drag", dragged)
                .on("end", dragended)
        );

        var labelNode = container.append("g").attr("class", "labelNodes")
            .selectAll("text")
            .data(label.nodes)
            .enter()
            .append("text")
            .text(function (d, i) {
                let labelText;
                if (d.node.Title.length > 20)
                    labelText = (d.node.Title).substring(0, 20) + "...";
                else
                    labelText = d.node.Title;
                return i % 2 == 0 ? "" : labelText;
            })
            .style("fill", "#203256")
            .style("font-family", "Open Sans")
            .style("font-size", 18)
            .attr("vector-effect", "non-scaling-stroke")
            .style("paint-order", "stroke")
            .style("stroke", "white")
            .style("stroke-width", 3)
            .style("stroke-linecap", "butt")
            .style("stroke-linejoin", "miter")
            .style("pointer-events", "none");

        node.on("mouseover", focus).on("mouseout", unfocus);

        function ticked() {
            node.call(updateNode);
            link.call(updateLink);

            labelLayout.alphaTarget(0.3).restart();
            labelNode.each(function (d, i) {
                if (i % 2 == 0) {
                    d.x = d.node.x;
                    d.y = d.node.y;
                } else {
                    var b = this.getBBox();

                    var diffX = d.x - d.node.x;
                    var diffY = d.y - d.node.y;

                    var dist = Math.sqrt(diffX * diffX + diffY * diffY);

                    var shiftX = b.width * (diffX - dist) / (dist * 2);
                    shiftX = Math.max(-b.width, Math.min(0, shiftX));
                    var shiftY = 16;
                    this.setAttribute("transform", "translate(" + shiftX + "," + shiftY + ")");
                }
            });
            labelNode.call(updateNode);
        };

        function fixna(x) {
            if (isFinite(x)) return x;
            return 0;
        };

        function focus(d) {
            var index = d3.select(d3.event.target).datum().index;
            node.style("opacity", function (o) {
                return neigh(index, o.index) ? 1 : 0.1;
            });
            labelNode.attr("display", function (o) {
                return neigh(index, o.node.index) ? "block" : "";
            });
            labelNode.attr("visibility", function (o) {
                return neigh(index, o.node.index) ? "" : "hidden";
            });
            link.style("opacity", function (o) {
                return o.source.index == index || o.target.index == index ? 1 : 0.1;
            });
        };

        function unfocus() {
            labelNode.attr("display", "block");
            labelNode.attr("visibility", "");
            node.style("opacity", 1);
            link.style("opacity", 1);
        };

        function updateLink(link) {
            link.attr("x1", function (d) { return fixna(d.source.x); })
                .attr("y1", function (d) { return fixna(d.source.y); })
                .attr("x2", function (d) { return fixna(d.target.x); })
                .attr("y2", function (d) { return fixna(d.target.y); });
        };

        function updateNode(node) {
            node.attr("transform", function (d) {
                return "translate(" + fixna(d.x) + "," + fixna(d.y) + ")";
            });
        };

        function dragstarted(d) {
            d3.event.sourceEvent.stopPropagation();
            if (!d3.event.active) graphLayout.alphaTarget(0.3).restart();
            d.fx = d.x;
            d.fy = d.y;
        };

        function dragged(d) {
            d.fx = d3.event.x;
            d.fy = d3.event.y;
        };

        function dragended(d) {
            if (!d3.event.active) graphLayout.alphaTarget(0);
            d.fx = null;
            d.fy = null;
        };
    }

    static async loadRectangleNodeGraph() {
        currentGraph = 'rectangleNodeGraph';

        'use strict';
         // INIT
        var windowWidth = 810,
            windowHeight = 600;

        var width = windowWidth,
            height = windowHeight;

        var color = d3.scaleOrdinal(d3.schemeCategory10);

        var svg = d3.select('#graph-body');
        svg.append("rect")
            .attr("width", "100%")
            .attr("height", "100%")
            .attr("fill", "white");

        var svgContainer = svg.append('g')
            .attr('id', 'svgContainer')
            .attr('transform', 'translate(' + width / 2 + ',' + height / 2 + ')');

        svg
            .call(d3.zoom()
                .scaleExtent([0.1, 4])
                .on('zoom', zoomed))
            .call(d3.zoom().transform, d3.zoomIdentity.translate(width / 2, height / 2))
            .on('dblclick.zoom', null);



        // from http://bl.ocks.org/rkirsling/5001347
        // define arrow markers for graph links

        var nodes = svgContainer.selectAll('g');
        var links = svgContainer.selectAll('g');

        var linkPreview = svgContainer
            .append('path');

        var vertices = <any>[];
        var edges = [];

        var source = null,
            target = null,
            link = null; // rename this (represents an edge)

        var dragging = true;

        var simulation = d3.forceSimulation()
            .force('x', d3.forceX(0))
            .force('y', d3.forceY(0))
            .force('link', d3.forceLink())
            .force('charge', d3.forceManyBody().strength(-200))
            .on('tick', tick);

        simulation.force('x').strength(0.02);
        simulation.force('y').strength(0.03);


        if (!dataIsReady)
            await this.limitGraphNodes();
        importGraph();

        await update();

        nodes.selectAll('text')
            .text(function(d) { return d.Title; })
            .attr('height', 10)
            .attr('width', 10)
            .attr('transform', function () {
                var b = this.getBBox();
                return 'translate(-' + b.width / 2 + ',' + 10 / 2 + ')';
            })
            .attr('style', 'cursor: default');
        nodes.selectAll('rect')
            .each(function (d) {
                let textBox = this.parentNode.querySelector('text');
                if (textBox == null) {
                    return;
                } else {
                    let b = this.parentNode.querySelector('text').getBBox();
                    d.width = b.width + 5;
                    d.height = 20;
                    d3.select(this)
                        .attr('width', d.width)
                        .attr('height', d.height)
                        .attr('transform', 'translate(-' + d.width / 2 + ',' + -10 + ')')
                        .attr('stroke', color(d.type))
                        .style("fill", "#fff")
                        .attr('rx', 5)
                        .attr('ry', 5);
                }
            });

        if (showKnowledgeBar) {

            let knowledgeBar = {
                'height': 10,
                'width': 1.5,
                'yPos': 12,
            }

            nodes.append('rect')
                .attr('class', 'solidKnowledgeBar')
                .attr('y', knowledgeBar.yPos)
                .attr('x', function (d) {
                    let b = this.previousSibling.getBBox();
                    d.width = b.width + 5;
                    return - d.width / 2;
                })
                .attr('height', knowledgeBar.height)
                .attr('width', function (d) {
                    if (d.Knowledge.SolidPercentage >= 0)
                        return (knowledgeBar.width * d.Knowledge.SolidPercentage) + 1;
                    else
                        return 0;
                })
                .style('fill', '#afd534')
                .style('fill-opacity', function (d) {
                    if (d.Knowledge.SolidPercentage > 0)
                        return 1;
                    else
                        return 0;
                });


            nodes.append('rect')
                .attr('class', 'needsConsolidationKnowledgeBar')
                .attr('y', knowledgeBar.yPos)
                .attr('x', function () {
                    let b = this.previousSibling.getBBox();
                    return b.x + b.width - 1;
                })
                .attr('height', knowledgeBar.height)
                .attr('width', function (d) {
                    if (d.Knowledge.NeedsConsolidationPercentage >= 0)
                        return (knowledgeBar.width * d.Knowledge.NeedsConsolidationPercentage) + 1;
                    else
                        return 0;
                })
                .style('fill', '#fdd648')
                .style('fill-opacity', function (d) {
                    if (d.Knowledge.NeedsConsolidationPercentage > 0)
                        return 1;
                    else
                        return 0;
                });

            nodes.append('rect')
                .attr('class', 'needsLearningKnowledgeBar')
                .attr('y', knowledgeBar.yPos)
                .attr('x', function () {
                    let b = this.previousSibling.getBBox();
                    return b.x + b.width - 1;
                })
                .attr('height', knowledgeBar.height)
                .attr('width', function (d) {
                    if (d.Knowledge.NeedsLearningPercentage >= 0)
                        return (knowledgeBar.width * d.Knowledge.NeedsLearningPercentage) + 1;
                    else
                        return 0;
                })
                .style('fill', 'lightsalmon')
                .style('fill-opacity', function (d) {
                    if (d.Knowledge.NeedsLearningKnowledge > 0)
                        return 1;
                    else
                        return 0;
                });

            nodes.append('rect')
                .attr('class', 'NotLearnedPercentage')
                .attr('y', knowledgeBar.yPos)
                .attr('x', function () {
                    let b = this.previousSibling.getBBox();
                    return b.x + b.width - 1;
                })
                .attr('height', knowledgeBar.height)
                .attr('width', function (d) {
                    if (d.Knowledge.NotLearnedPercentage >= 0)
                        return (knowledgeBar.width * d.Knowledge.NotLearnedPercentage) + 1;
                    else
                        return 0;
                })
                .style('fill', 'silver')
                .style('fill-opacity', function (d) {
                    if (d.Knowledge.NotLearnedPercentage > 0)
                        return 1;
                    else
                        return 0;
                });

            nodes.append('rect')
                .attr('class', 'NotInWishknowledgePercentage')
                .attr('y', knowledgeBar.yPos)
                .attr('x', function () {
                    let b = this.previousSibling.getBBox();
                    return b.x + b.width - 1;
                })
                .attr('height', knowledgeBar.height)
                .attr('width', function (d) {
                    if (d.Knowledge.Total > 0)
                        return (knowledgeBar.width * d.Knowledge.NotInWishknowledgePercentage);
                    else
                        return 0;
                })
                .style('fill', '#dddddd')
                .style('fill-opacity', function (d) {
                    if (d.Knowledge.Total > 0)
                        return 1;
                    else
                        return 0;
                });
        }

        function update() {
            links = links.data(edges, function (d) {
                return d.index;
            });
            links.exit().remove();
            var enter = links.enter().append('g')
                .on('mouseover', linkHover)
                .on('mouseout', linkUnHover);
            enter.append('path')
                .style('marker-end', 'url(#end-arrow)');
            links = links.merge(enter);

            nodes = nodes.data(vertices, function (d) {
                return d.Id;
            });
            nodes.exit().remove();
            enter = nodes.enter().append('g')
                .on('mouseover', nodeHover)
                .on('mouseout', nodeUnHover)
                .call(d3.drag()
                    .on('start', nodeDragStart)
                    .on('drag', nodeDragProgress)
                    .on('end', nodeDragEnd)
                );
            enter.append('text');
            enter.append('rect');

            nodes = nodes.merge(enter);

            nodes.classed('selected', function (d) {
                return d.selected;
            });
            links.classed('selected', function (d) {
                return d.selected;
            })
                    .attr("stroke", "#999")
                    .attr("stroke-width", "1px");

            links.lower();
            d3.selectAll('text').raise();

            simulation.nodes(vertices);
            simulation.force('link')
                .links(edges)
                .distance(100)
                .strength(0.2);
        }

        function tick() {

            links.each(drawPath);

            nodes.attr('transform', function (d) {
                return 'translate(' + d.x + ',' + d.y + ')';
            });
        }

        function drawPath(d) {
            let path;
            if (this.children) {
                path = this.children[0];
            } else {
                path = this.node();
            }

            const x1 = d.source.x;
            const y1 = d.source.y;
            const y2 = d.target.y;
            const x2 = d.target.x;
            const w2 = d.target.width / 2 + 3;
            const h2 = d.target.height / 2 + 3;

            const dx = x1 - x2;
            const dy = y1 - y2;
            const m12 = dy / dx;
            const m2 = h2 / w2;

            let x2a;
            let y2a;

            if (Math.abs(m12) > Math.abs(m2)) {
                // if slope of link is greater than aspect ratio of box
                // link exits out the bottom
                x2a = x2 + dy / Math.abs(dy) * h2 / m12;
                y2a = y2 + dy / Math.abs(dy) * h2;
            } else {
                // link exits out the side
                x2a = x2 + dx / Math.abs(dx) * w2;
                y2a = y2 + dx / Math.abs(dx) * w2 * m12;
            }

            d3.select(path)
                .attr('d', `
                  M ${x1} ${y1}
                  L ${x2a} ${y2a}
                `);
        }

        function nodeDragStart(d) {
            source = d;
            dragging = true; // dragging the bldg
        }

        function nodeDragProgress(d) {
            source.fx = d3.event.x;
            source.fy = d3.event.y;

                simulation.alpha(0.3).restart();
        }

        function nodeDragEnd(d) {
            linkPreview
                .style('visibility', 'hidden');

            if (!d.fixed) {
                source.fx = null;
                source.fy = null;
            }
            update();
        }


        function nodeHover(d) {
            target = d;
            d3.select(this).attr('stroke-width', 3);
        }

        function nodeUnHover(d) {
            target = null;
            d3.select(this).attr('stroke-width', 1);
        }

        function linkHover(d) {
            link = d;
        }

        function linkUnHover(d) {
            link = null;
        }

        function importGraph() {
            // TODO: check for duplicate IDs

            vertices = graphNodes;
            edges = graphLinks;

            update();
            simulation.alpha(1).restart();
        }

        function zoomed() {
            svgContainer.attr('transform', d3.event.transform);
        }
    }
}