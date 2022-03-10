Vue.component('session-progress-bar-component',
    {
        template: '#session-progress-bar-template',
        props: [],
        data() {
            return {
                currentStep: 0,
                steps: 0,
                progress: 0,
                progressBackground: ''
            };
        },

        mounted() {
            eventBus.$on('set-session-progress',
                (e) => {
                    this.currentStep = e.currentStepIdx + 1;
                    this.steps = e.stepCount;
                });
        },

        watch: {
            steps() {
                this.updateProgress();
            },
            currentStep() {
                this.updateProgress();
            }
        },

        methods: {
            updateProgress() {
                this.progress = (100 / this.steps * this.currentStep).toFixed();
                var progressBarValue = (100 / this.steps * (this.currentStep - 1)).toFixed();
                this.progressBackground = `background: linear-gradient(90deg, #AFD534 ${progressBarValue}%, #EFEFEF ${progressBarValue}%)`;
            }
        }
    });