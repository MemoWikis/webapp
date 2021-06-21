
Vue.component('numeric-component', {
    props: ['solution'],
        data() {
            return {
                number: '',
            }
    },

    mounted() {
        if (this.solution)
            this.number = this.solution;
    },

    methods: {
        setSolution() {
            let metadataJson = { IsNumber: true };

            this.$parent.numericSolution = this.number;
            this.$parent.solutionMetadataJson = metadataJson;
        }
    }
})