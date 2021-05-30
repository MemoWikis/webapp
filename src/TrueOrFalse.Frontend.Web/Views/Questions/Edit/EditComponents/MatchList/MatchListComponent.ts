
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

    mounted() {
        if (this.solution)
            this.initiateSolution();
    },

    methods: {
        initiateSolution() {
            this.pairs = JSON.parse(this.solution);
        },
        addPair() {
            let placeHolder = {
                ElementRight: { Text: "" },
                ElementLeft: { Text: "" }
            }
            this.pairs.push(placeHolder);
        },
        deletePair(index) {
            this.pairs.splice(index, 1);
        },
        addRightElement() {
            let rightElement = { Text: "" }
            this.rightElements.push(rightElement);
        },
        deleteRightElement(index) {
            this.rightElements.splice(index, 1);
        },
        answerBuilder() {
            let solution = {
                Pairs: this.pairs,
                RightElements: this.rightElements,
                IsSolutionOrdered: this.isSolutionOrdered
            }
            this.$parent.matchListJson = solution;
        }
    }
})