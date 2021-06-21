
Vue.component('date-component', {
    props: ['solution'],
        data() {
            return {
                date: '',
                precision: [],
                seletedPrecision: ''
            }
    },

    mounted() {
        if (this.solution)
            this.date = this.solution;
        this.dateEnumToArray();
    },

    methods: {
        dateEnumToArray() {
            var stringIsNotANumber = value => isNaN(Number(value));
            this.precision = Object.keys(DatePrecision)
                .filter(stringIsNotANumber)
                .map(key => 
                    SolutionMetadataDate.GetPrecisionLabel(DatePrecision[key])
                );
        },
        setSolution() {
            let metadataJson = {
                IsDate: true,
                Precision: this.selectedPrecision
            }

            this.$parent.dateSolution = this.date;
            this.$parent.solutionMetadataJson = metadataJson;
        }
    }
})