
Vue.component('multiplechoice-singlesolution-component', {
    props: ['solution'],
    data() {
        return {
            choices: [{
                value: '',
            }],
        }
    },
    watch: {
        choices() {
            this.trim();
        }
    },
    mounted() {
        if (this.solution.length > 0)
            this.initiateSolution();
    },

    methods: {
        initiateSolution() {
            var choices = JSON.parse(this.solution).Choices;
            var formattedChoices = [];
            choices.forEach(choice => formattedChoices.push({ value: choice }));
            this.choices = formattedChoices;
        },
        trim() {
            let trimmedChoices = [];

            this.choices.forEach(choice => {
                trimmedChoices.push(choice.value);
                }
            );
            var solution = {
                Choices: trimmedChoices
            }
            this.$parent.singleSolutionJson = solution;

        },
        addChoice() {
            let placeHolder = {
                value: '',
            }
            this.choices.push(placeHolder);
        },
        deleteChoice(index) {
            this.choices.splice(index, 1);
        }
    }
})