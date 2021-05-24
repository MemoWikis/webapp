
Vue.component('multiplechoice-component', {
    props: ['solution'],
    data() {
        return {
            pairs: [{
                ElementRight: { Text: "" },
                ElementLeft: { Text: "" }
            }],
            isSolutionOrdered: false,
        }
    },

    mounted() {
        if (this.solution.length > 0)
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
        answerBuilder() {
            this.answer = {
                Pairs: this.pairs,
                IsSolutionOrdered: this.isSolutionOrdered
            }
        }
    }
})