
Vue.component('multiplechoice-singlesolution-component', {
        props: ['current-category-id','answer'],
        data() {
            return {
                choices: [{
                    value: '',
                }],
                trimmedChoices: []
            }
        },

    methods: {
        trim() {
            this.choices.forEach(choice => {
                    this.trimmedChoices.push(choice.value);
                }
            );
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