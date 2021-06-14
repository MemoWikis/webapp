
Vue.component('textsolution-component', {
    props: ['solution'],
        data() {
            return {
                text: '',
                exactSpelling: false,
                matchCase: false,
            }
    },

    watch: {
        text(val) {
            this.$parent.solutionIsValid = val.length > 0;
            this.setSolution();
        }
    },
    mounted() {
        if (this.solution)
            this.text = this.solution;
    },

    methods: {
        setSolution() {
            let metadataJson = { IsText: true };

            this.$parent.textSolution = this.text;
            this.$parent.solutionMetadataJson = metadataJson;

        }
    }
})