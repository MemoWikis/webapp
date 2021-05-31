
Vue.component('matchlist-component', {
    props: ['solution'],
    data() {
        return {
            pairs: [{
                ElementRight: { Text: "" },
                ElementLeft: { Text: "" }
            }],
            rightElements: [{ Text: "" }],
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
            this.pairs = JSON.parse(this.solution);
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
        }
    }
})