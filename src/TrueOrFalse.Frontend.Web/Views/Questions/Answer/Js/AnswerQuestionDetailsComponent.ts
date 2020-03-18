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
        personalProbability: function () {
            this.updateData();
        },
    },

    mounted: function () {
        this.drawSemiPie();
    },

    methods: {

        updateData() {
            var semiPieRef = this.$refs.semiPie;
            var semiPie = d3.select(semiPieRef).transition();

            var personalKnowledgeState = this.personalProbability / 100;
            var avgKnowledgeState = this.averageProbability / 100;

            var arcKnowledgeState = d3.arc()
                .innerRadius(45)
                .outerRadius(60)
                .startAngle(-0.5 * this.pi)
                .endAngle((-0.5 + personalKnowledgeState) * this.pi);

            var arcAvg = d3.arc()
                .innerRadius(42)
                .outerRadius(62)
                .startAngle((-0.5 + avgKnowledgeState) * this.pi - 0.01)
                .endAngle((-0.5 + avgKnowledgeState) * this.pi + 0.01);

            semiPie.select(".arcPersonal")
                .attr("d", arcKnowledgeState);

            semiPie.select(".arcAvg")
                .attr("d", arcAvg);
        },

        drawSemiPie() {

            var semiPieRef = this.$refs.semiPie;
            var semiPie = d3.select(semiPieRef).append("svg");

            var personalKnowledgeState = this.personalProbability / 100;
            var avgKnowledgeState = this.averageProbability / 100;

            var arcBase = d3.arc()
                .innerRadius(50)
                .outerRadius(55)
                .startAngle(-0.5 * this.pi)
                .endAngle(0.5 * this.pi);

            var arcKnowledgeState = d3.arc()
                .innerRadius(45)
                .outerRadius(60)
                .startAngle(-0.5 * this.pi)
                .endAngle((-0.5 + personalKnowledgeState) * this.pi);

            var arcAvg = d3.arc()
                .innerRadius(42)
                .outerRadius(62)
                .startAngle((-0.5 + avgKnowledgeState) * this.pi - 0.01)
                .endAngle((-0.5 + avgKnowledgeState) * this.pi + 0.01);

            semiPie.attr("width", "400").attr("height", "400")
                .append("path")
                .attr("d", arcBase)
                .attr("class", "arcBase")
                .attr("fill", "grey")
                .attr("transform", "translate(200,200)");

            semiPie.append("path")
                .attr("d", arcKnowledgeState)
                .attr("class", "arcPersonal")
                .attr("fill", "red")
                .attr("transform", "translate(200,200)");

            semiPie.append("path")
                .attr("d", arcAvg)
                .attr("class", "arcAvg")
                .attr("fill", "black")
                .attr("transform", "translate(200,200)");
        }
    },
});