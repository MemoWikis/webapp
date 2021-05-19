
Vue.component('multiplechoice-component', {
        props: ['current-category-id','answer'],
        data() {
            return {
                pairs: [{
                    ElementRight: { Text: "" },
                    ElementLeft: { Text: "" }
                }],
                isSolutionOrdered: false,
            }
        },

    methods: {
        addPair() {
            let placeHolder = {
                ElementRight: { Text: "" },
                ElementLeft: { Text: "" }
            }
            this.pairs.push(placeHolder);
        },
        deletePair(index) {
            this.pairs.splice(index, 1);
        }
    }
})