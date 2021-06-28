
Vue.component('textsolution-component', {
    props: ['solution', 'highlightEmptyFields'],
    data() {
        return {
            text: '',
            exactSpelling: false,
            matchCase: false,
            highlightEmptyField: false,
            isEmpty: '',
        }
    },

    mounted() {
        if (this.solution)
            this.text = this.solution;
    },

    watch: {
        text(val) {
            this.$parent.solutionIsValid = val.length > 0;
            this.setSolution();

            if (val.length == 0)
                this.isEmpty = 'is-empty';
            else
                this.isEmpty = '';
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