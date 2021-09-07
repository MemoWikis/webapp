
Vue.component('textsolution-component', {
    props: ['solution', 'highlightEmptyFields'],
    template: '#textsolution-template',
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

        this.$parent.solutionIsValid = this.text.length > 0;
    },

    watch: {
        text(val) {
            this.$parent.solutionIsValid = val.length > 0;
            this.setSolution();

            this.isEmpty = val.length == 0 ? 'is-empty' : '';
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