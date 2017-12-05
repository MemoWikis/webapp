class CategoryHeader {

    constructor() {
        this.drawViewsByDayChart();
    }

    drawViewsByDayChart(){

        var data = [
            { date: "2013-01", value: 23 },
            { date: "2013-02", value: 7 },
            { date: "2013-03", value: 1 },
            { date: "2013-04", value: 35 },
            { date: "2013-05", value: 23 },
            { date: "2013-06", value: 2 },
            { date: "2013-07", value: 4 },
            { date: "2013-08", value: 1 },
            { date: "2013-09", value: 0 },
            { date: "2013-10", value: 4 },
            { date: "2013-11", value: 2 }
        ];

        var margin = {top: 20, right: 20, bottom: 70, left: 40},
            width = 300 - margin.left - margin.right,
            height = 170 - margin.top - margin.bottom;

        var x = d3.scaleBand().rangeRound([0, width]).padding(0.1),
            y = d3.scaleLinear().rangeRound([height, 0]);

        var g = d3.select("#last60DaysViews")
            .append("svg")
            .append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        x.domain(data.map(function(d) { return <any>d.date; }));
        y.domain([0, d3.max(data, function(d) { return d.value; })]);

        g.append("g")
            .attr("class", "axis axis--x")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x))      
                .selectAll("text")	
                .style("text-anchor", "end")
                .attr("dx", "-.8em")
                .attr("dy", ".15em")
                .attr("transform", "rotate(-90) translate(0, -7)");

        g.append("g")
            .attr("class", "axis axis--y")
            .call(d3.axisLeft(y).ticks(10))
        .append("text")
            .attr("transform", "rotate(-90)")
            .attr("y", 6)
            .attr("dy", "0.71em")
            .attr("text-anchor", "end")
            .text("Date");

        g.selectAll(".bar")
        .data(data)
        .enter()
            .append("rect")
            .attr("class", "bar")
            .attr("x", function(d) { return x(d.date); })
            .attr("y", function(d) { return y(<any>d.value); })
            .attr("width", x.bandwidth())
            .attr("height", function(d) { return height - y(<any>d.value); });
    }
}