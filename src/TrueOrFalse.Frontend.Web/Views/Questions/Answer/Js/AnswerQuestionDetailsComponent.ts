declare var d3: any;

Vue.component('question-details-component', {

    props: {
        questionId: Number,
        isOpen: Boolean,
    },
    template: '#question-details-component',

    data() {
        return {
            personalProbability: 0,
            personalColor: "",
            avgProbability: 0,
            personalAnswerCount: 0,
            personalAnsweredCorrectly: 0,
            avgAnswerCount: 0,
            avgAnsweredCorrectly: 0,
            isLoggedIn: IsLoggedIn.Yes,
            showTopBorder: false,
            showCategoryList: false,
            svg: {},

            baseArcData: {
                startAngle: -0.55 * Math.PI,
                endAngle: 0.55 * Math.PI,
                innerRadius: 45,
                outerRadius: 50,
                fill: "#DDDDDD",
            },

            personalArcData: {},

            avgArcData: {},

            baseArcPath: "",
            personalArcPath: "",
            avgArcPath: "",

            arc: d3.arc(),
            testData: "",
            arcLoaded: false,
            percentageLabelWidth: 0,
            avgAngle: 0,
            dxAvgLabel: 0,
            dyAvgLabel: 0,
            avgLabelAnchor: "middle",
            avgProbabilityLabelWidth: 0,
        };
    },

    watch: {
        personalProbability: function () {
            this.setPersonalArcData();
            if (this.arcLoaded)
                this.updateArc();
        },

        avgProbability: async function () {
            this.setAvgArcData();
            if (this.arcLoaded) {
                await this.updateArc();
            }
        },

        isOpen: function(val) {
            if (val) {
                this.getStats();
            }

        },
    },

    computed: {
    },

    mounted: function () {
    },

    methods: {
        clearData() {
            var self = this;

            var data = [
                this.baseArcData,
                this.personalArcData,
                this.avgArcData
            ];

            var selection = self.svg.selectAll("path")
                .data(data);

            selection.exit().remove();
        },
        setPersonalArcData() {
            this.personalArcData = {
                startAngle: -0.55 * Math.PI,
                endAngle: (-0.55 + this.personalProbability / 100 * 1.1) * Math.PI,
                innerRadius: 40,
                outerRadius: 55,
                fill: this.personalColor,
                class: "personalArc",
            };
            return true;
        },
        setAvgArcData() {
            var avgInnerRadius = 37.5;
            var avgOuterRadius = 57.5;

            if (this.personalProbability < this.avgProbability) {
                avgInnerRadius = 42.5;
                avgOuterRadius = 52.5;
            }

            this.avgArcData = {
                startAngle: (-0.55 + this.avgProbability / 100 * 1.1) * Math.PI - 0.01,
                endAngle: (-0.55 + this.avgProbability / 100 * 1.1) * Math.PI + 0.01,
                innerRadius: avgInnerRadius,
                outerRadius: avgOuterRadius,
                fill: "#707070",
                class: "avgArc"
            };

            return true;
        },

        setAvgLabelPos() {
            var self = this;
            self.avgAngle = (-0.55 + this.avgProbability / 100 * 1.1) * Math.PI;

            var probabilityData = [self.avgProbability];

            self.svg.append('g')
                .selectAll('.dummyAvgProbability')
                .data(probabilityData)
                .enter()
                .append("text")
                .attr("font-family", "font-family:Lato")
                .attr("font-weight", "regular")
                .attr("font-size", "12")
                .text(function (d) { return "∅ " + d + "%" })
                .each(function () {
                    var thisWidth = this.getComputedTextLength();
                    self.avgProbabilityLabelWidth = thisWidth;
                    this.remove();
                });

            self.dxAvgLabel = (self.avgProbabilityLabelWidth) * ((-50 + self.avgProbability) / 100) * 0.75;
            var el = (self.avgProbability - 50) / 10;
            self.dyAvgLabel = (((0.20 * Math.pow(el, 2) - 5) * 1.2) + 3) * 2.5;

            if (self.avgProbability > 50)
                self.avgLabelAnchor = "start";
            else if (self.avgProbability < 50)
                self.avgLabelAnchor = "end";
        },

        getStats() {
            $.ajax({
                url: "/AnswerQuestion/GetQuestionDetails/",
                data: { questionId: this.questionId },
                type: "Post",
                success: async data => {
                    this.personalProbability = data.personalProbability;
                    this.personalColor = data.personalColor;
                    this.avgProbability = data.avgProbability;
                    this.personalAnswerCount = data.personalAnswerCount;
                    this.personalAnsweredCorrectly = data.personalAnsweredCorrectly;
                    this.avgAnswerCount = data.avgAnswerCount;
                    this.avgAnsweredCorrectly = data.avgAnsweredCorrectly;

                    await this.setPersonalArcData();
                    await this.setAvgArcData();
                    if (!this.arcLoaded)
                        this.drawArc();
                    else
                        this.updateArc();
                }
            });
        },

        drawArc() {

            var width = 400;
            var height = 200;
            var self = this;

            var semiPieRef = self.$refs.semiPie;

            self.svg = d3.select(semiPieRef).append("svg")
                .attr("width", width)
                .attr("height", height)
                .append("g").attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

            var arc = d3.arc();

            var data = [
                this.baseArcData,
                this.personalArcData,
                this.avgArcData
            ];

            self.svg.selectAll("path").data(data).enter()
                .append("path")
                .style("fill", function (d) { return d.fill })
                .attr("class", function (d) { return d.class })
                .attr("d", arc);

            this.drawProbabilityLabel();
            this.setAvgLabel();

            this.arcLoaded = true;
        },

        setAvgLabel() {
            var self = this;

            this.setAvgLabelPos();

            var pos = d3.arc()
                .innerRadius(55)
                .outerRadius(55)
                .startAngle(self.avgAngle)
                .endAngle(self.avgAngle);

            self.svg.append("svg:text")
                .attr("transform", function (d) {
                    return "translate(" + pos.centroid(d) + ")";
                })
                .attr("dx", self.dxAvgLabel)
                .attr("dy", self.dyAvgLabel)
                .attr("text-anchor", self.avgLabelAnchor)
                .attr("style", "font-family:Lato")
                .attr("font-size", "12")
                .attr("font-weight", "regular")
                .attr("fill", "#555555")
                .attr("class", "avgProbabilityLabel")
                .text("∅ "+ self.avgProbability + "%");
        },

        drawProbabilityLabel() {
            var self = this;
            var labelWidth = this.calculateLabelWidth();

            self.svg.append("svg:text")
                .attr("dy", ".1em")
                .attr("dx", -(labelWidth / 2) + "px")
                .attr("text-anchor", "left")
                .attr("style", "font-family:Lato")
                .attr("font-size", "30")
                .attr("font-weight", "bold")
                .attr("fill", self.personalColor)
                .attr("class", "personalProbabilityLabel")
                .text(self.personalProbability);

            self.svg.append("svg:text")
                .attr("dy", "-.35em")
                .attr("dx", (labelWidth / 2) - self.percentageLabelWidth - 1 + "px")
                .attr("style", "font-family:Lato")
                .attr("text-anchor", "left")
                .attr("font-size", "18")
                .attr("font-weight", "medium")
                .attr("class", "percentageLabel")
                .attr("fill", self.personalColor)
                .text("%");
        },

        calculateLabelWidth() {
            var self = this;

            var probabilityLabelWidth = 0;

            var probabilityAsText = [self.personalProbability];

            self.svg.append('g')
                .selectAll('.dummyProbability')
                .data(probabilityAsText)
                .enter()
                .append("text")
                .attr("font-family", "font-family:Lato")
                .attr("font-weight", "bold")
                .attr("font-size", "30px")
                .text(function (d) { return d })
                .each(function () {
                    var thisWidth = this.getComputedTextLength();
                    probabilityLabelWidth = thisWidth;
                    this.remove();
                });

            self.svg.append('g')
                .selectAll('.dummyPercentage')
                .data("%")
                .enter()
                .append("text")
                .attr("font-family", "font-family:Lato")
                .attr("font-weight", "medium")
                .attr("font-size", "18px")
                .text(function (d) { return d })
                .each(function () {
                    var thisWidth = this.getComputedTextLength();
                    self.percentageLabelWidth = thisWidth;
                    this.remove();
                });

            return probabilityLabelWidth + self.percentageLabelWidth + 1;
        },

        updateArc() {
            var self = this;
            var labelWidth = this.calculateLabelWidth();

            self.svg.selectAll(".personalProbabilityLabel")
                .transition()
                .duration(600)
                .attr("dx", -(labelWidth / 2) + "px")
                .style("fill", self.personalColor)
                .tween("text", function () {
                    var selection = d3.select(this);
                    var start = d3.select(this).text();
                    var end = self.personalProbability;
                    var interpolator = d3.interpolateNumber(start, end);

                    return function (t) { selection.text(Math.round(interpolator(t))); };
                });

            self.svg.selectAll(".avgProbabilityLabel")
                .transition()
                .duration(600)
                .tween("text", function () {
                    var selection = d3.select(this);
                    var text = d3.select(this).text();
                    var numbers = text.match(/(\d+)/);
                    var end = self.avgProbability;
                    var interpolator = d3.interpolateNumber(numbers[0], end);

                    return function (t) { selection.text("∅ " + Math.round(interpolator(t)) + "%"); };
                });

            self.svg.selectAll(".percentageLabel").transition()
                .duration(600)
                .attr("dx", (labelWidth / 2) - self.percentageLabelWidth + "px")
                .style("fill", self.personalColor);

            self.svg.selectAll(".personalArc").transition()
                .duration(1000)
                .style("fill", self.personalColor )
                .attrTween("d", function (d) { return self.arcTween(d, self.personalArcData.startAngle, self.personalArcData.endAngle, self.personalArcData.innerRadius, self.personalArcData.outerRadius) });

            self.svg.selectAll(".avgArc").transition()
                .duration(1000)
                .attrTween("d", function (d) { return self.arcTween(d, self.avgArcData.startAngle, self.avgArcData.endAngle, self.avgArcData.innerRadius, self.avgArcData.outerRadius) });
        },

        arcTween(d, newStartAngle, newEndAngle, newInnerRadius, newOuterRadius) {
            var arc = d3.arc();

            var interpolateStart = d3.interpolate(d.startAngle, newStartAngle);
            var interpolateRadiusStart = d3.interpolate(d.innerRadius, newInnerRadius);
            var interpolateEnd = d3.interpolate(d.endAngle, newEndAngle);
            var interpolateRadiusEnd = d3.interpolate(d.outerRadius, newOuterRadius);
            return function (t) {
                d.innerRadius = interpolateRadiusStart(t);
                d.outerRadius = interpolateRadiusEnd(t);
                d.startAngle = interpolateStart(t);
                d.endAngle = interpolateEnd(t);
                return arc(d);
            }
        },
    },
});