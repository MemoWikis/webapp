
Vue.component('multiplechoice-component', {
    props: ['solution'],
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
        if (this.solution.length > 0)
            this.initiateSolution();
    },

    methods: {
        initiateSolution() {
            this.choices = JSON.parse(this.solution).Choices;
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
        answerBuilder() {
            this.answer = {
                Choices: this.choices,
                IsSolutionOrdered: this.isSolutionOrdered
            }
        }
    }
})