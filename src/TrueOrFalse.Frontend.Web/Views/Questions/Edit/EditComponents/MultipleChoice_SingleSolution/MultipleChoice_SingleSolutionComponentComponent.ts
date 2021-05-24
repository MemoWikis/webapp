
Vue.component('multiplechoice-singlesolution-component', {
    props: ['solution'],
    data() {
        return {
            choices: [{
                value: '',
            }],
            trimmedChoices: []
        }
    },
    mounted() {
        if (this.solution.length > 0)
            this.initiateSolution();
    },

    methods: {
        initiateSolution() {
            var choices = JSON.parse(this.solution).Choices;
            console.log(choices);
            var formattedChoices = [];
            choices.forEach(choice => formattedChoices.push({ value: choice }));
            this.choices = formattedChoices;
        },
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