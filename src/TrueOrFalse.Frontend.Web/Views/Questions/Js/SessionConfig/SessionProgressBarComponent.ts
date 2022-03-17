Vue.component('session-progress-bar-component',
    {
        template: '#session-progress-bar-template',
        props: [],
        data() {
            return {
                currentStep: 0,
                steps: 0,
                progress: 0,
                progressBarWidth: '',
                showProgressBar: false,
            };
        },

        mounted() {
            eventBus.$on('set-session-progress',
                (e) => {
                    if (e == null)
                        this.showProgressBar = false;
                    else if (e.isResult ){
                        this.showProgressBar = false;
                    }
                    else {
                        this.currentStep = e.currentStepIdx + 1;
                        this.steps = e.stepCount;
                        this.showProgressBar = true;
                    }
                });
            eventBus.$on('update-progress-bar',
                () => {
                    this.progress = (100 / this.steps * this.currentStep).toFixed();
                    this.progressBarWidth = `width: ${this.progress}%`;
                });
            eventBus.$on('init-new-session',
                () => {
                    this.progressBarWidth = `width: 0%`;
                    this.showProgressBar = true;
                });

            this.showProgressBar = true;
        },

        watch: {
            steps() {
                this.updateSteps();
            },
            currentStep() {
                this.updateSteps();
            },
        },

        methods: {
            updateSteps() {
                if (this.currentStep == this.steps)
                    $('#hddIsLearningSession').attr('data-is-last-step', 'true');
                else
                    $('#hddIsLearningSession').attr('data-is-last-step', 'false');
            }
        }
    });