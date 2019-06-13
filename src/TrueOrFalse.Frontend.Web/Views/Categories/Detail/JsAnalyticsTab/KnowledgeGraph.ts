declare var label: any;
declare var graphData: any;

class KnowledgeGraph {

    static loadForceGraph() {

        var width = 810;
        var height = 600;
        var color = d3.scaleOrdinal(d3.schemeCategory10);

        var graph = graphData;

        var label = {
            'nodes': [],
            'links': [],
        };

        graph.nodes.forEach(function(d, i) {
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

        var graphLayout = d3.forceSimulation(graph.nodes)
            .force("charge", d3.forceManyBody().strength(-3000))
            .force("center", d3.forceCenter(width / 2, height / 2))
            .force("x", d3.forceX(width / 2).strength(1))
            .force("y", d3.forceY(height / 2).strength(1))
            .force("link", d3.forceLink(graph.links).distance(100).strength(2))
            .on("tick", ticked);

        var adjlist = [];

        graph.links.forEach(function (d) {
            adjlist[d.source.index + "-" + d.target.index] = true;
            adjlist[d.target.index + "-" + d.source.index] = true;
        });

        function neigh(a, b) {
            return a == b || adjlist[a + "-" + b];
        }

        var svg = d3.select("#graph-body").attr("width", width).attr("height", height);
        var container = svg.append("g");

        svg.call(
            d3.zoom()
                .scaleExtent([.1, 4])
                .on("zoom", function () { container.attr("transform", d3.event.transform); })
        );

        var link = container.append("g").attr("class", "links")
            .selectAll("line")
            .data(graph.links)
            .enter()
            .append("line")
            .attr("stroke", "#999")
            .attr("stroke-width", "1px");

        var node = container.append("g").attr("class", "nodes")
            .selectAll("g")
            .data(graph.nodes)
            .enter()
            .append("circle")
            .attr("r", function (d) {
                d.weight = link.filter(function (l) {
                    return l.source.index == d.index || l.target.index == d.index;
                }).size();
                var minRadius = 5;
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
                if (d.node.Text.length > 20)
                    labelText = (d.node.Text).substring(0, 20) + "...";
                else
                    labelText = d.node.Text;
                return i % 2 == 0 ? "" : labelText;
            })
            .style("fill", "#203256")
            .style("font-family", "Open Sans")
            .style("font-size", 18)
            .style("paint-order", "stroke")
            .style("stroke", "white")
            .style("stroke-width", "4px")
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
                return neigh(index, o.node.index) ? "block" : "none";
            });
            link.style("opacity", function (o) {
                return o.source.index == index || o.target.index == index ? 1 : 0.1;
            });
        };

        function unfocus() {
            labelNode.attr("display", "block");
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



    static loadSimpleForceGraph() {

        var windowWidth = 810,
            windowHeight = 600;

        var width = windowWidth - 258,
            height = windowHeight - 10;

        var color = d3.scaleOrdinal(d3.schemeCategory10);

        var svg = d3.select('body').append('svg')
            .attr('width', width)
            .attr('height', height);

        var world = svg.append('g')
            .attr('id', 'world')
            .attr('transform', 'translate(' + width / 2 + ',' + height / 2 + ')');

        svg
            .call(d3.zoom()
                .scaleExtent([1 / 8, 2])
                .on('zoom', zoomed))
            .call(d3.zoom().transform, d3.zoomIdentity.translate(width / 2, height / 2))
            .on('dblclick.zoom', null);

        // from http://bl.ocks.org/rkirsling/5001347
        // define arrow markers for graph links
        svg.append('svg:defs').append('svg:marker')
            .attr('id', 'end-arrow')
            .attr('viewBox', '0 -5 15 10')
            .attr('refX', 7.5)
            .attr('markerWidth', 7.5)
            .attr('markerHeight', 5)
            .attr('orient', 'auto')
            .append('svg:path')
            .attr('d', 'M 0 -5 L 10 0 L 0 5')
            .attr('style', 'fill: #000; stroke: none');

        var buildings = world.selectAll('g');
        var lines = world.selectAll('g');

        var linePreview = world
            .append('path');

        var vertices = [];
        var edges = [];

        d3.text(graphData).then(function (g) {
            importGraph(g);
        });

        var source = null,
            target = null,
            selected = null,
            line = null; // rename this (represents an edge)

        var dragging = true;
        var ctrlPressed = false;

        window.addEventListener("resize", resize);

        svg.on('dblclick', newVertexAtMouse);
        svg.on('click', selectObj);

        d3.select(window)
            .on('keydown', windowKeydown)
            .on('keyup', windowKeyup);

        var simulation = d3.forceSimulation()
            .force('x', d3.forceX(0))
            .force('y', d3.forceY(0))
            .force('link', d3.forceLink().id(function (d) { return d.id; }))
            .force('charge', d3.forceManyBody().strength(-200))
            .on('tick', tick);

        simulation.force('x').strength(0.02);
        simulation.force('y').strength(0.03);

        update();

        function update() {
            lines = lines.data(edges, function (d) {
                return d.index;
            });
            lines.exit().remove();
            var enter = lines.enter().append('g')
                .on('click', selectObj)
                .on('mouseover', lineHover)
                .on('mouseout', lineUnHover);
            enter.append('path')
                .style('marker-end', 'url(#end-arrow)');
            lines = lines.merge(enter);
        
            buildings = buildings.data(vertices, function (d) {
                return d.id;
            });
            buildings.exit().remove();
            enter = buildings.enter().append('g')
                .on('click', selectObj)
                .on('mouseover', bldgHover)
                .on('mouseout', bldgUnHover)
                .call(d3.drag()
                    .on('start', bldgDragStart)
                    .on('drag', bldgDragProgress)
                    .on('end', bldgDragEnd)
                );
            enter.append('text');
            enter.append('rect');
        
            buildings = buildings.merge(enter);
        
            buildings.classed('selected', function (d) {
                return d.selected;
            });
            lines.classed('selected', function (d) {
                return d.selected;
            });
        
            buildings.selectAll('text')
                .text(function (d) { return d.title; })
                .attr('height', 10)
                .attr('transform', function () {
                    var b = this.getBBox();
                    return 'translate(-' + b.width / 2 + ',' + 10 / 2 + ')';
                });
            buildings.selectAll('rect')
                .each(function (d) {
                    const b = this.parentNode.querySelector('text').getBBox();
                    d.width = b.width + 5;
                    d.height = 20;
                    d3.select(this)
                        .attr('width', d.width)
                        .attr('height', d.height)
                        .attr('transform', 'translate(-' + d.width / 2 + ',' + -10 + ')')
                        .attr('stroke', color(d.type))
                        .attr('rx', 5)
                        .attr('ry', 5);
                });
        
            lines.lower();
            d3.selectAll('text').raise();
        
            simulation.nodes(vertices);
            simulation.force('link')
                .links(edges)
                .distance(100)
                .strength(0.2);
        }
        
        function tick() {
            lines.each(drawPath);
        
            buildings.attr('transform', function (d) {
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
                // if slope of line is greater than aspect ratio of box
                // line exits out the bottom
                x2a = x2 + dy / Math.abs(dy) * h2 / m12;
                y2a = y2 + dy / Math.abs(dy) * h2;
            } else {
                // line exits out the side
                x2a = x2 + dx / Math.abs(dx) * w2;
                y2a = y2 + dx / Math.abs(dx) * w2 * m12;
            }
        
            d3.select(path)
                .attr('d', `
              M ${x1} ${y1}
              L ${x2a} ${y2a}
            `);
        }
        
        function newVertexAtMouse() {
            var x = d3.mouse(world.node())[0];
            var y = d3.mouse(world.node())[1];
            newVertex(x, y);
        }
        
        function newVertex(x = 0, y = 0) {
            var lastVertexId = 0;
        
            vertices.forEach(function (vertex) {
                if (vertex.id > lastVertexId) {
                    lastVertexId = vertex.id;
                }
            });
        
            var vertex = { id: ++lastVertexId, title: 'New', type: '', x: x, y: y };
        
            vertices.push(vertex);
            selectObj(vertex);
        
            update();
            simulation.alpha(0.3).restart();
            return vertex;
        }
        
        function deleteObj(obj) {
            if (vertices.indexOf(obj) !== -1) {
                vertices.splice(vertices.indexOf(obj), 1);
        
                edges = edges.filter(function (edge) {
                    return (edge.source !== obj && edge.target !== obj);
                });
            }
        
            if (edges.indexOf(obj) !== -1) {
                edges.splice(edges.indexOf(obj), 1);
            }
        
            update();
            simulation.alpha(0.3).restart();
        }
        
        function bldgDragStart(d) {
            source = d;
        
            if (!ctrlPressed) {
                dragging = true; // dragging the bldg
            } else {
                dragging = false; // drawing new edge
            }
        }
        
        function bldgDragProgress(d) {
            var tx, ty;
            if (dragging) {
                source.fx = d3.event.x;
                source.fy = d3.event.y;
            } else {
                if (target && target !== source) {
                    drawPath.call(linePreview, { source, target });
                } else {
                    tx = d3.mouse(world.node())[0];
                    ty = d3.mouse(world.node())[1];
                    linePreview
                        .style('display', 'inline')
                        .style('marker-end', 'url(#end-arrow)')
                        .attr('d', function (d) {
                            return `M ${source.x} ${source.y} L ${tx} ${ty}`
                        });
                }
            }
        
            simulation.alpha(0.3).restart();
        }
        
        function bldgDragEnd(d) {
            linePreview
                .style('display', 'none');
        
            if (dragging) {
                if (!d.fixed) {
                    source.fx = null;
                    source.fy = null;
                }
            } else {
                if (target) {
                    if (source !== target && !edgeExists(source, target)) {
                        edges.push({ source: source, target: target });
                        updateInspector(selected);
                    }
                }
            }
        
            update();
        }
        
        function bldgHover(d) {
            target = d;
            d3.select(this).classed('hover', true);
        }
        
        function bldgUnHover(d) {
            target = null;
            d3.select(this).classed('hover', false);
        }
        
        function lineHover(d) {
            line = d;
        }
        
        function lineUnHover(d) {
            line = null;
        }
        
        function selectObj(subject) {
            if (d3.event) {
                d3.event.stopPropagation();
            }
        
            if (subject === selected) {
                // TODO: re-implement for multi-select
                //       do not interfere with dblclick
                //    subject = null;
            }
        
            selected = subject;
        
            edges.forEach(function (edge) {
                edge.selected = false;
            });
        
            vertices.forEach(function (vertex) {
                vertex.selected = false;
            });

            update();
        }
        
        function fixBldg(subject) {
            if (subject && subject.fixed) {
                d3.select(this).classed('fixed', false);
                subject.fixed = false;
                subject.fx = null;
                subject.fy = null;
                simulation.alpha(0.3).restart();
            } else if (subject) {
                d3.select(this).classed('fixed', true);
                subject.fixed = true;
                subject.fx = subject.x;
                subject.fy = subject.y;
            }
        }
        
        function edgeExists(source, target) {
            for (var i = 0; i < edges.length; i++) {
                if (source === edges[i].source && target === edges[i].target) {
                    return true;
                }
            }
        }
        
        function exportGraph() {
            var cleanEdges = edges.map(function (edge) {
                var cleanEdge = Object.assign({}, edge);
                cleanEdge.source = cleanEdge.source.id;
                cleanEdge.target = cleanEdge.target.id;
                return cleanEdge;
            });
        
            var cleanGraph = { vertices: vertices, edges: cleanEdges };
            return JSON.stringify(cleanGraph);
        }
        
        function importGraph(dirtyGraph) {
            // TODO: check for duplicate IDs
            var graph = JSON.parse(dirtyGraph);
        
            vertices = [];
            edges = [];
        
            vertices = graph.vertices;
            edges = graph.edges;
            update();
            simulation.alpha(1).restart();
        }
        
        function windowKeydown(d) {
            if (d3.event.target.type !== "text") {
                switch (d3.event.keyCode) {
                    case 16: // shift
                    case 17: // ctrl
                    case 18: // alt
                    case 91: // cmd
                        ctrlPressed = true;
                        svg.attr('style', 'cursor: pointer');
                        break;
                    case 8:  // backspace
                    case 46: // delete
                    case 68: // d
                        d3.event.preventDefault();
                        world.selectAll('.selected').each(deleteObj);
                        target = null;
                        break;
                    case 80: // p
                        world.selectAll('.selected').each(fixBldg);
                        break;
                    case 27: // esc
                        selectObj(null);
                        break;
                    default:
                        break;
                }
            }
        }
        
        function windowKeyup() {
            switch (d3.event.keyCode) {
                case 16: // shift
                case 17: // ctrl
                case 18: // alt
                case 91: // cmd
                    ctrlPressed = false;
                    svg.attr('style', 'cursor: default');
                    break;
                default:
                    break;
            }
        }
        
        function resize() {
            var windowWidth = window.innerWidth,
                windowHeight = window.innerHeight;
        
            width = windowWidth - 258,
                height = windowHeight - 10;
        
            svg
                .attr('width', width)
                .attr('height', height);
        
            simulation
                .alpha(0.3).restart();
        }
        
        function zoomed() {
            world.attr('transform', d3.event.transform);
        }

    }
}