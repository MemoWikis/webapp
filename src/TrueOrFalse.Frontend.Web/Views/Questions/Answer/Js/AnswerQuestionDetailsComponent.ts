declare var d3: any;

Vue.component('question-details-component', {

    props: {
        questionId: Number,
        isOpen: Boolean,
    },
    template: '#question-details-component',

    data() {
        return {
            personalProbability: 67,
            personalColor: "",
            avgProbability: 90,
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
        };
    },

    watch: {
        personalProbability: function () {
            this.setPersonalArcData();
        },

        avgProbability: function () {
            this.setAvgArcData();
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

            var width = 200;
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
                .attr("class", function (d) { return d.class})
                .attr("d", arc);

            this.arcLoaded = true;
        },

        updateArc() {
            var self = this;

            self.svg.selectAll(".personalArc").transition()
                .duration(1000)
                .style("fill", function () { return self.personalColor })
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