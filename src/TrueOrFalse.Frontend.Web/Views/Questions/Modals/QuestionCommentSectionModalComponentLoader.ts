Vue.component('question-comment-section-modal-component',
    {
        template: '#question-comment-section-modal-component',
        props: [],
        data() {
            return {
                commentIsLoaded: true,
                questionId: 6338
        }
        },
        created() {
            const self = this;
            console.log(self.questionId);
            eventBus.$on('closeModal', function () {
                self.commentIsLoaded = false;
            });
        },
    });
