declare var d3: any;

Vue.component('question-details-component', {

    //props: {
    //    questionId: Number,
    //    showCategoryList: Boolean,
    //},
    template: '#question-details-component',

    data() {
        return {
            personalProbability: 67,
            averageProbability: 90,
            personalAnswerCount: 0,
            answerCount: 0,
            pi: Math.PI,
            isLoggedIn: IsLoggedIn.Yes,
            showTopBorder: false,
            showCategoryList: false,
            svg: {},

            arcBaseData: {
                startAngle: -0.5 * Math.PI,
                endAngle: 0.5 * Math.PI,
                innerRadius: 60,
                outerRadius: 65,
            },

            arcPersonalData: {},

            arcAvgData: {},

            baseArcPath: "",
            personalArcPath: "",
            avgArcPath: "",
        };
    },

    watch: {
        personalProbability: function(val) {
        },
    },

    computed: {
        baseArc: function () {
            var arcGenerator = d3.arc();
            return this.baseArcPath = arcGenerator({
                startAngle: -0.5 * Math.PI,
                endAngle: 0.5 * Math.PI,
                innerRadius: 60,
                outerRadius: 65,
            });
        },
        personalArc: function() {
            var arcGenerator = d3.arc();
            return this.personalArcPath = arcGenerator({
                startAngle: -0.5 * this.pi,
                endAngle: (-0.5 + this.personalProbability / 100) * this.pi,
                innerRadius: 55,
                outerRadius: 70,
                fill: "red",
                class: "personalArc",
            });
        },
        avgArc: function () {
            var arcGenerator = d3.arc();
            return this.avgArcPath = arcGenerator({
                startAngle: (-0.5 + this.averageProbability / 100) * this.pi - 0.01,
                endAngle: (-0.5 + this.averageProbability / 100) * this.pi + 0.01,
                innerRadius: 54,
                outerRadius: 71,
                fill: "black",
                class: "avg"
            });
        },
    },

    mounted: function () {
        this.arcPersonalData = {
            startAngle: -0.5 * this.pi,
            endAngle: (-0.5 + this.personalProbability / 100) * this.pi,
            innerRadius: 55,
            outerRadius: 70,
            fill: "red",
            class: "personalArc",
        };
        this.arcAvgData = {
            startAngle: (-0.5 + this.averageProbability / 100) * this.pi - 0.01,
            endAngle: (-0.5 + this.averageProbability / 100) * this.pi + 0.01,
            innerRadius: 54,
            outerRadius: 71,
            fill: "black",
            class: "avg"
        };
        //this.testPie();
    },

    methods: {

        updateData(val) {

            var width = 960
            var height = 500

            var arcColors = ["#F3A54A", "#AA7CAA"];

            var personalArc = this.$refs.semiPie;

            var selection = d3.select(personalArc).selectAll("path")

            //this.svg = d3.select(semiPieRef).append("svg")
            //    .attr("width", width)
            //    .attr("height", height)
            //    .append("g").attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

            var self = this;

            var arc = d3.arc();

            var data = [this.arcBaseData, this.arcPersonalData, this.arcAvgData];

            console.log(this.$refs.semiPie)
            console.log("______________")
            console.log(this.arcPersonalData)

            var path = selection.enter().data(data)
                .append("path")
                .style("fill", function (d) { return d.fill })
                .attr("d", arc)
                .attr("class", function (d) { return d.class })
                .attr("ref", function (d) { return d.ref });

            path.enter().append("path")
                .style("fill", function (d) { return d.fill; })
                .attr("d", arc)
                .each(function (d) { this._current = d; });

            path.transition()
                .attrTween("d", arcTween);

            path.exit().remove()

            function arcTween(a) {
                var i = d3.interpolate(this._current, a);
                this._current = i(0);
                return function(t) {
                    return arc(i(t));
                };
            }
        },



        //drawSemiPie() {

        //    var semiPieRef = this.$refs.semiPie;
        //    var semiPie = d3.select(semiPieRef).append("svg");

        //    var personalKnowledgeState = this.personalProbability / 100;
        //    var avgKnowledgeState = this.averageProbability / 100;

        //    var arcBase = d3.arc()
        //        .innerRadius(50)
        //        .outerRadius(55)
        //        .startAngle(-0.5 * this.pi)
        //        .endAngle(0.5 * this.pi);

        //    var arcKnowledgeState = d3.arc()
        //        .innerRadius(45)
        //        .outerRadius(60)
        //        .startAngle(-0.5 * this.pi);

        //    var arcAvg = d3.arc()
        //        .innerRadius(42)
        //        .outerRadius(62)
        //        .startAngle((-0.5 + avgKnowledgeState) * this.pi - 0.01)
        //        .endAngle((-0.5 + avgKnowledgeState) * this.pi + 0.01);

        //    semiPie.attr("width", "400").attr("height", "400")
        //        .append("path")
        //        .attr("d", arcBase)
        //        .attr("class", "arcBase")
        //        .attr("fill", "grey")
        //        .attr("transform", "translate(200,200)");

        //    semiPie.append("path")
        //        .datum({ endAngle: (-0.5 + personalKnowledgeState) * this.pi })
        //        .attr("d", arcKnowledgeState)
        //        .attr("class", "arcPersonal")
        //        .attr(":ref", "arcPersonal")
        //        .attr("fill", "red")
        //        .attr("transform", "translate(200,200)");

        //    semiPie.append("path")
        //        .attr("d", arcAvg)
        //        .attr("class", "arcAvg")
        //        .attr("fill", "black")
        //        .attr("transform", "translate(200,200)");

        //    var tau = 2 * Math.PI;

        //    d3.interval(function () {
        //        arcKnowledgeState.transition()
        //            .duration(750)
        //            .attrTween("d", arcTween(Math.random() * tau));
        //    }, 1500);

        //    var randomNumber = (-0.5 + personalKnowledgeState) * this.pi;

        //    function arcTween(newAngle) {
        //        return function (d) {
        //            var interpolate = d3.interpolate(d.endAngle, newAngle);
        //            return function (t) {
        //                d.endAngle = interpolate(t);
        //                return arcKnowledgeState(d);
        //            };
        //        };
        //    };
        //},

        testPie() {
            var width = 960
            var height = 500

            var arcColors = ["#F3A54A", "#AA7CAA"];

            var semiPieRef = this.$refs.semiPie;

            this.svg = d3.select(semiPieRef).append("svg")
                .attr("width", width)
                .attr("height", height)
                .append("g").attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

            var self = this;

            var arc = d3.arc();

            var data = [this.arcBaseData, this.arcPersonalData, this.arcAvgData];

            var path = self.svg.selectAll("path").data(data).enter()
                .append("path")
                .style("fill", function (d) { return d.fill })
                .attr("d", arc)
                .attr("class", function (d) { return d.class })
                .attr("ref", function (d) { return d.ref });

            path.transition()
                .duration(2000)
                .attrTween("d", function (d) { return arcTween(d, 0.7 * Math.random()) })

            var arcGenerator = d3.arc();

            var pathData = arcGenerator({
                startAngle: 0,
                endAngle: 0.25 * Math.PI,
                innerRadius: 50,
                outerRadius: 100
            });

            console.log(pathData);


            function arcTween(d, new_score) {
                var new_startAngle = Math.random() * 2 * Math.PI
                var new_endAngle = new_startAngle + new_score * 2 * Math.PI
                var interpolate_start = d3.interpolate(d.startAngle, new_startAngle)
                var interpolate_end = d3.interpolate(d.endAngle, new_endAngle)
                return function (t) {
                    d.startAngle = interpolate_start(t)
                    d.endAngle = interpolate_end(t)
                    return arc(d)
                }
            }


        }
    },
});