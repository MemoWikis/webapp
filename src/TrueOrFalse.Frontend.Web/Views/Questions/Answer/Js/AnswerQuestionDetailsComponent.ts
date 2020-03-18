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
        };
    },

    watch: {
    },

    mounted: function () {
        this.drawSemiPie();
    },

    methods: {

        drawSemiPie() {

            var semiPieRef = this.$refs.semiPie;
            var semiPie = d3.select(semiPieRef).append("svg");

            var personalKnowledgeState = this.personalProbability / 100;
            var avgKnowledgeState = this.averageProbability / 100;

            var endAngle = 0.5 * this.pi;

            var arcBase = d3.svg.arc()
                .innerRadius(50)
                .outerRadius(55)
                .startAngle(-endAngle)
                .endAngle(endAngle);

            var arcKnowledgeState = d3.svg.arc()
                .innerRadius(45)
                .outerRadius(60)
                .startAngle(endAngle)
                .endAngle(endAngle + personalKnowledgeState);

            var arcAvg = d3.svg.arc()
                .innerRadius(42)
                .outerRadius(62)
                .startAngle(avgKnowledgeState * (this.pi / 180) - .5)
                .endAngle(avgKnowledgeState * (this.pi / 180) + .5);

            semiPie.attr("width", "400").attr("height", "400")
                .append("path")
                .attr("d", arcBase)
                .attr("fill", "grey")
                .attr("transform", "translate(200,200)");

            semiPie.append("path")
                .attr("d", arcKnowledgeState)
                .attr("fill", "red")
                .attr("transform", "translate(200,200)");

            semiPie.append("path")
                .attr("d", arcAvg)
                .attr("fill", "black")
                .attr("transform", "translate(200,200)");
        }
    },
});