
Vue.component('textsolution-component', {
    props: ['solution'],
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
        highlightEmptyField(val) {
            if (val)
                this.isEmpty = 'is-empty';
            else this.isEmpty = '';
        },
        text(val) {
            this.$parent.solutionIsValid = val.length > 0;
            this.setSolution();

            if (val.length == 0)
                this.highlightEmptyField = true;
            else
                this.highlightEmptyField = false;
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