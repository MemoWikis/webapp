
Vue.component('matchlist-component', {
    props: ['solution'],
    data() {
        return {
            pairs: [{
                ElementRight: { Text: "" },
                ElementLeft: { Text: "" }
            }],
            rightElements: [],
            isSolutionOrdered: false,
        }
    },

    watch: {
    },

    mounted() {
        if (this.solution)
            this.initiateSolution();
    },

    methods: {
        initiateSolution() {
            var json = JSON.parse(this.solution);
            this.pairs = json.Pairs;
            this.rightElements = json.RightElements;
            this.isSolutionOrdered = json.IsSolutionOrdered;
            this.solutionBuilder();
        },
        addPair() {
            let placeHolder = {
                ElementRight: { Text: "" },
                ElementLeft: { Text: "" }
            }
            this.pairs.push(placeHolder);
            this.solutionBuilder();
        },
        deletePair(index) {
            this.pairs.splice(index, 1);
            this.solutionBuilder();
        },
        addRightElement() {
            let rightElement = { Text: "" }
            this.rightElements.push(rightElement);
            this.solutionBuilder();
        },
        deleteRightElement(index) {
            this.rightElements.splice(index, 1);
            this.solutionBuilder();
        },
        solutionBuilder() {
            let solution = {
                Pairs: this.pairs,
                RightElements: this.rightElements,
                IsSolutionOrdered: this.isSolutionOrdered
            }
            this.$parent.matchListJson = solution;
        },
        validateSolution() {
            var hasEmptyAnswer = this.rightElements.some((e) => {
                return e.Text.trim() == '';
            });
            var leftElementHasNoAnswer = this.pairs.some((p) => {
                return p.ElementLeft.Text.trim() == '';
            });
            var rightElementHasNoAnswer = this.pairs.some((p) => {
                return p.ElementRight.Text.trim() == '';
            });
            this.$parent.solutionIsValid = !hasEmptyAnswer && !leftElementHasNoAnswer && !rightElementHasNoAnswer;
        }
    }
})