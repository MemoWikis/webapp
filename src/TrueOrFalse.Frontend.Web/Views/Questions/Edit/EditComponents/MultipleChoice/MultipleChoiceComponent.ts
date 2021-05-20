
Vue.component('multiplechoice-component', {
        props: ['current-category-id','answer'],
        data() {
            return {
                choices: [{
                        Text: '',
                        IsCorrect: true
                }],
                isSolutionOrdered: false,
            }
        },

    methods: {
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