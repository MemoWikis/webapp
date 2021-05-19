
Vue.component('multiplechoice-component', {
        props: ['current-category-id','answer'],
        data() {
            return {
                choices: [{
                        Text: '',
                        IsCorrect: true
                    }],
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
        }
    }
})