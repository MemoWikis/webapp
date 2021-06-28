
Vue.component('multiplechoice-component', {
    props: ['solution','highlightEmptyFields'],
    data() {
        return {
            choices: [{
                    Text: '',
                    IsCorrect: true
            }],
            isSolutionOrdered: false,
        }
    },

    mounted() {
        if (this.solution)
            this.initiateSolution();
    },

    watch: {
        choices() {
            this.solutionBuilder();
        },
        isSolutionOrdered() {
            this.solutionBuilder();
        }
    },

    methods: {
        initiateSolution() {
            var json = JSON.parse(this.solution);
            this.choices = json.Choices;
            this.isSolutionOrdered = json.IsSolutionOrdered;
            this.validateSolution();
        },
        updateElement(index, newVal) {
            this.choices[index] = newVal;
        },
        addChoice() {
            let placeHolder = {
                Text: '',
                IsCorrect: false
            }
            this.choices.push(placeHolder);
        },
        deleteChoice(index) {
            this.choices.splice(index, 1);
        },
        toggleCorrectness(index) {
            this.choices[index].IsCorrect = !this.choices[index].IsCorrect;
            this.solutionBuilder();
        },
        solutionBuilder() {
            this.validateSolution();

            let solution = {
                Choices: this.choices,
                IsSolutionOrdered: this.isSolutionOrdered
            }

            this.$parent.multipleChoiceJson = solution;
        },
        validateSolution() {
            var hasEmptyAnswer = this.choices.some((c) => {
                return c.Text.trim() == '';
            });
            this.$parent.solutionIsValid = !hasEmptyAnswer;
        }
    }
})