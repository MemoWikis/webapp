
Vue.component('textsolution-component', {
    props: ['solution'],
        data() {
            return {
                text: '',
                exactSpelling: false,
                matchCase: false,
            }
    },

    mounted() {
        if (this.solution)
            this.text = this.solution.slice(1, -1);
    },

    watch: {
        text(val) {
            this.$parent.solutionIsValid = val.length > 0;
            this.setSolution();
        }
    },


    methods: {
        setSolution() {
            let metadataJson = { IsText: true };

            this.$parent.textSolution = this.text;
            this.$parent.solutionMetadataJson = metadataJson;

        }
    }
})